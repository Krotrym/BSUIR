#include <algorithm>
#include <chrono>
#include <cmath>
#include <cstdlib>
#include <cstring>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <string>
#include <vector>

#ifdef __APPLE__
#include <OpenCL/opencl.h>
#else
#include <CL/cl.h>
#endif

namespace {

struct Config {
    double a = -1.0;
    double b = 1.0;
    double h = 0.1;
    double eps = 1e-8;
    int terms = 10000;
    int iterations = 20000;
    int batch = 1024;
    int platform_index = 0;
    int device_index = 0;
    std::string out = "lab3_opencl.csv";
};

bool arg_exists(int argc, char** argv, const char* key) {
    for (int i = 1; i < argc; ++i) {
        if (std::strcmp(argv[i], key) == 0) {
            return true;
        }
    }
    return false;
}

const char* arg_value(int argc, char** argv, const char* key) {
    for (int i = 1; i + 1 < argc; ++i) {
        if (std::strcmp(argv[i], key) == 0) {
            return argv[i + 1];
        }
    }
    return nullptr;
}

double get_double(int argc, char** argv, const char* key, double def) {
    const char* value = arg_value(argc, argv, key);
    return value ? std::atof(value) : def;
}

int get_int(int argc, char** argv, const char* key, int def) {
    const char* value = arg_value(argc, argv, key);
    return value ? std::atoi(value) : def;
}

std::string get_string(int argc, char** argv, const char* key, std::string def) {
    const char* value = arg_value(argc, argv, key);
    return value ? std::string(value) : def;
}

void print_help(const char* exe) {
    std::cout
        << "Usage: " << exe << " [options]\n"
        << "  --a <value>             interval start, default -1\n"
        << "  --b <value>             interval end, default 1\n"
        << "  --h <value>             interval step, default 0.1\n"
        << "  --eps <value>           precision for n, default 1e-8\n"
        << "  --terms <value>         series terms per work item, default 10000\n"
        << "  --iterations <value>    GPU work items, default 20000\n"
        << "  --batch <value>         work items per kernel launch, default 1024\n"
        << "  --platform <index>      OpenCL platform index, default 0\n"
        << "  --device <index>        OpenCL device index, default 0\n"
        << "  --out <file.csv>        output CSV, default lab3_opencl.csv\n";
}

Config parse_args(int argc, char** argv) {
    Config cfg;
    if (arg_exists(argc, argv, "--help")) {
        print_help(argv[0]);
        std::exit(0);
    }
    cfg.a = get_double(argc, argv, "--a", cfg.a);
    cfg.b = get_double(argc, argv, "--b", cfg.b);
    cfg.h = get_double(argc, argv, "--h", cfg.h);
    cfg.eps = get_double(argc, argv, "--eps", cfg.eps);
    cfg.terms = get_int(argc, argv, "--terms", cfg.terms);
    cfg.iterations = get_int(argc, argv, "--iterations", cfg.iterations);
    cfg.batch = get_int(argc, argv, "--batch", cfg.batch);
    cfg.platform_index = get_int(argc, argv, "--platform", cfg.platform_index);
    cfg.device_index = get_int(argc, argv, "--device", cfg.device_index);
    cfg.out = get_string(argc, argv, "--out", cfg.out);

    if (cfg.h <= 0.0) {
        throw std::runtime_error("--h must be positive");
    }
    if (cfg.b < cfg.a) {
        throw std::runtime_error("--b must be greater than or equal to --a");
    }
    if (cfg.terms <= 5000) {
        throw std::runtime_error("--terms must be greater than 5000");
    }
    if (cfg.iterations <= 5000) {
        throw std::runtime_error("--iterations must be greater than 5000");
    }
    if (cfg.batch <= 0) {
        throw std::runtime_error("--batch must be positive");
    }
    return cfg;
}

void check(cl_int err, const char* what) {
    if (err != CL_SUCCESS) {
        std::ostringstream os;
        os << what << " failed with OpenCL error " << err;
        throw std::runtime_error(os.str());
    }
}

std::string platform_info(cl_platform_id platform, cl_platform_info param) {
    size_t size = 0;
    check(clGetPlatformInfo(platform, param, 0, nullptr, &size), "clGetPlatformInfo(size)");
    std::string value(size, '\0');
    check(clGetPlatformInfo(platform, param, size, value.data(), nullptr), "clGetPlatformInfo");
    if (!value.empty() && value.back() == '\0') {
        value.pop_back();
    }
    return value;
}

std::string device_info(cl_device_id device, cl_device_info param) {
    size_t size = 0;
    check(clGetDeviceInfo(device, param, 0, nullptr, &size), "clGetDeviceInfo(size)");
    std::string value(size, '\0');
    check(clGetDeviceInfo(device, param, size, value.data(), nullptr), "clGetDeviceInfo");
    if (!value.empty() && value.back() == '\0') {
        value.pop_back();
    }
    return value;
}

std::vector<float> make_points(double a, double b, double h) {
    std::vector<float> xs;
    for (double x = a; x <= b + h * 0.5; x += h) {
        xs.push_back(static_cast<float>(x));
    }
    return xs;
}

const char* kernel_source() {
    return R"CLC(
__kernel void series_kernel(__global const float* xs,
                            const int xs_count,
                            const int terms,
                            const float eps,
                            __global float* sums,
                            __global int* n_values)
{
    const int gid = (int)get_global_id(0);
    const float x = xs[gid % xs_count];
    const float two_x = 2.0f * x;
    const float two_x_sq = two_x * two_x;

    float sum = 0.0f;
    float term = 0.0f;
    int n_eps = 0;

    for (int k = 1; k <= terms; ++k) {
        if (k == 1) {
            term = -two_x_sq / 2.0f;
        } else {
            const float denom = (float)(2 * k) * (float)(2 * k - 1);
            term *= -two_x_sq / denom;
        }
        sum += term;
        const float abs_term = term < 0.0f ? -term : term;
        if (n_eps == 0 && abs_term < eps) {
            n_eps = k;
        }
    }

    sums[gid] = sum;
    n_values[gid] = n_eps;
}
)CLC";
}

cl_device_id choose_device(const Config& cfg, cl_platform_id& chosen_platform) {
    cl_uint platform_count = 0;
    check(clGetPlatformIDs(0, nullptr, &platform_count), "clGetPlatformIDs(count)");
    if (platform_count == 0) {
        throw std::runtime_error("no OpenCL platforms found");
    }

    std::vector<cl_platform_id> platforms(platform_count);
    check(clGetPlatformIDs(platform_count, platforms.data(), nullptr), "clGetPlatformIDs");
    if (cfg.platform_index < 0 || cfg.platform_index >= static_cast<int>(platforms.size())) {
        throw std::runtime_error("bad OpenCL platform index");
    }
    chosen_platform = platforms[static_cast<std::size_t>(cfg.platform_index)];

    cl_uint device_count = 0;
    cl_int err = clGetDeviceIDs(chosen_platform, CL_DEVICE_TYPE_GPU, 0, nullptr, &device_count);
    cl_device_type requested_type = CL_DEVICE_TYPE_GPU;
    if (err != CL_SUCCESS || device_count == 0) {
        requested_type = CL_DEVICE_TYPE_ALL;
        check(clGetDeviceIDs(chosen_platform, requested_type, 0, nullptr, &device_count),
              "clGetDeviceIDs(count)");
    }

    std::vector<cl_device_id> devices(device_count);
    check(clGetDeviceIDs(chosen_platform, requested_type, device_count, devices.data(), nullptr),
          "clGetDeviceIDs");
    if (cfg.device_index < 0 || cfg.device_index >= static_cast<int>(devices.size())) {
        throw std::runtime_error("bad OpenCL device index");
    }
    return devices[static_cast<std::size_t>(cfg.device_index)];
}

} // namespace

int main(int argc, char** argv) {
    try {
        const Config cfg = parse_args(argc, argv);
        const std::vector<float> xs = make_points(cfg.a, cfg.b, cfg.h);

        cl_platform_id platform = nullptr;
        cl_device_id device = choose_device(cfg, platform);

        std::cout << "OpenCL platform: " << platform_info(platform, CL_PLATFORM_NAME) << "\n";
        std::cout << "OpenCL device: " << device_info(device, CL_DEVICE_NAME) << "\n";

        cl_int err = CL_SUCCESS;
        cl_context context = clCreateContext(nullptr, 1, &device, nullptr, nullptr, &err);
        check(err, "clCreateContext");

        cl_command_queue queue = clCreateCommandQueue(context, device, 0, &err);
        check(err, "clCreateCommandQueue");

        const char* source = kernel_source();
        const size_t source_size = std::strlen(source);
        cl_program program = clCreateProgramWithSource(context, 1, &source, &source_size, &err);
        check(err, "clCreateProgramWithSource");

        err = clBuildProgram(program, 1, &device, "-cl-std=CL1.2 -cl-opt-disable", nullptr, nullptr);
        if (err != CL_SUCCESS) {
            size_t log_size = 0;
            clGetProgramBuildInfo(program, device, CL_PROGRAM_BUILD_LOG, 0, nullptr, &log_size);
            std::string log(log_size, '\0');
            clGetProgramBuildInfo(program, device, CL_PROGRAM_BUILD_LOG, log_size, log.data(), nullptr);
            throw std::runtime_error("OpenCL build failed:\n" + log);
        }

        cl_kernel kernel = clCreateKernel(program, "series_kernel", &err);
        check(err, "clCreateKernel");

        cl_mem xs_buf = clCreateBuffer(context,
                                       CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR,
                                       xs.size() * sizeof(float),
                                       const_cast<float*>(xs.data()),
                                       &err);
        check(err, "clCreateBuffer(xs)");

        const int batch = std::min(cfg.batch, cfg.iterations);
        std::vector<float> sums(static_cast<std::size_t>(batch));
        std::vector<int> n_values(static_cast<std::size_t>(batch));

        cl_mem sums_buf = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
                                         sums.size() * sizeof(float), nullptr, &err);
        check(err, "clCreateBuffer(sums)");
        cl_mem n_buf = clCreateBuffer(context, CL_MEM_WRITE_ONLY,
                                      n_values.size() * sizeof(int), nullptr, &err);
        check(err, "clCreateBuffer(n)");

        std::ofstream csv(cfg.out);
        if (!csv) {
            throw std::runtime_error("cannot open output CSV");
        }
        csv << "elapsed_seconds,iterations,batch,checksum,avg_n\n";

        double checksum = 0.0;
        long long n_checksum = 0;
        const auto start = std::chrono::steady_clock::now();

        for (int done = 0; done < cfg.iterations; done += batch) {
            const int current_batch = std::min(batch, cfg.iterations - done);
            const size_t global = static_cast<size_t>(current_batch);
            const int xs_count = static_cast<int>(xs.size());

            check(clSetKernelArg(kernel, 0, sizeof(cl_mem), &xs_buf), "clSetKernelArg(xs)");
            check(clSetKernelArg(kernel, 1, sizeof(int), &xs_count), "clSetKernelArg(xs_count)");
            check(clSetKernelArg(kernel, 2, sizeof(int), &cfg.terms), "clSetKernelArg(terms)");
            const float eps = static_cast<float>(cfg.eps);
            check(clSetKernelArg(kernel, 3, sizeof(float), &eps), "clSetKernelArg(eps)");
            check(clSetKernelArg(kernel, 4, sizeof(cl_mem), &sums_buf), "clSetKernelArg(sums)");
            check(clSetKernelArg(kernel, 5, sizeof(cl_mem), &n_buf), "clSetKernelArg(n)");

            const size_t local = 1;
            check(clEnqueueNDRangeKernel(queue, kernel, 1, nullptr, &global, &local,
                                         0, nullptr, nullptr),
                  "clEnqueueNDRangeKernel");
            check(clFinish(queue), "clFinish");

            check(clEnqueueReadBuffer(queue, sums_buf, CL_TRUE, 0,
                                      current_batch * sizeof(float), sums.data(),
                                      0, nullptr, nullptr),
                  "clEnqueueReadBuffer(sums)");
            check(clEnqueueReadBuffer(queue, n_buf, CL_TRUE, 0,
                                      current_batch * sizeof(int), n_values.data(),
                                      0, nullptr, nullptr),
                  "clEnqueueReadBuffer(n)");

            for (int i = 0; i < current_batch; ++i) {
                checksum += sums[static_cast<std::size_t>(i)];
                n_checksum += n_values[static_cast<std::size_t>(i)];
            }

            const auto now = std::chrono::steady_clock::now();
            const std::chrono::duration<double> elapsed = now - start;
            const int total = done + current_batch;
            const double avg_n = static_cast<double>(n_checksum) / static_cast<double>(total);
            csv << elapsed.count() << ',' << total << ','
                << current_batch << ',' << checksum << ',' << avg_n << '\n';
        }

        std::cout << "CSV written to " << cfg.out << "\n";

        clReleaseMemObject(n_buf);
        clReleaseMemObject(sums_buf);
        clReleaseMemObject(xs_buf);
        clReleaseKernel(kernel);
        clReleaseProgram(program);
        clReleaseCommandQueue(queue);
        clReleaseContext(context);
        return 0;
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
}
