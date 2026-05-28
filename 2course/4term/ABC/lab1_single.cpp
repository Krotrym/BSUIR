#include <algorithm>
#include <chrono>
#include <cmath>
#include <cstdlib>
#include <cstring>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <stdexcept>
#include <string>
#include <vector>

namespace {

#if defined(__i386__) || defined(__x86_64__)
double asm_add(double a, double b) {
    double r;
    __asm__ __volatile__(
        "fldl %[a]\n\t"
        "faddl %[b]\n\t"
        "fstpl %[r]\n\t"
        : [r] "=m"(r)
        : [a] "m"(a), [b] "m"(b)
        : "st", "memory");
    return r;
}

double asm_mul(double a, double b) {
    double r;
    __asm__ __volatile__(
        "fldl %[a]\n\t"
        "fmull %[b]\n\t"
        "fstpl %[r]\n\t"
        : [r] "=m"(r)
        : [a] "m"(a), [b] "m"(b)
        : "st", "memory");
    return r;
}

double asm_div(double a, double b) {
    double r;
    __asm__ __volatile__(
        "fldl %[a]\n\t"
        "fdivl %[b]\n\t"
        "fstpl %[r]\n\t"
        : [r] "=m"(r)
        : [a] "m"(a), [b] "m"(b)
        : "st", "memory");
    return r;
}

double asm_cos(double x) {
    double r;
    __asm__ __volatile__(
        "fldl %[x]\n\t"
        "fcos\n\t"
        "fstpl %[r]\n\t"
        : [r] "=m"(r)
        : [x] "m"(x)
        : "st", "memory");
    return r;
}
#else
double asm_add(double a, double b) { return a + b; }
double asm_mul(double a, double b) { return a * b; }
double asm_div(double a, double b) { return a / b; }
double asm_cos(double x) { return std::cos(x); }
#endif

struct Config {
    double a = -1.0;
    double b = 1.0;
    double h = 0.1;
    double eps = 1e-8;
    int terms = 10000;
    int iterations = 20000;
    int checkpoint = 250;
    std::string out = "lab1_single.csv";
};

struct Result {
    double x = 0.0;
    double y = 0.0;
    double s = 0.0;
    int n_eps = 0;
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
        << "  --terms <value>         series terms per calculation, default 10000\n"
        << "  --iterations <value>    benchmark calculations, default 20000\n"
        << "  --checkpoint <value>    CSV checkpoint step, default 250\n"
        << "  --out <file.csv>        output CSV, default lab1_single.csv\n";
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
    cfg.checkpoint = get_int(argc, argv, "--checkpoint", cfg.checkpoint);
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
    if (cfg.checkpoint <= 0) {
        throw std::runtime_error("--checkpoint must be positive");
    }
    return cfg;
}

double y_exact(double x) {
    const double c = asm_cos(x);
    return asm_mul(2.0, asm_add(asm_mul(c, c), -1.0));
}

Result calculate_series(double x, int terms, double eps) {
    const double y = y_exact(x);
    const double two_x = asm_mul(2.0, x);
    const double two_x_sq = asm_mul(two_x, two_x);
    double sum = 0.0;
    double term = 0.0;
    int n_eps = 0;

    for (int k = 1; k <= terms; ++k) {
        if (k == 1) {
            term = asm_div(-two_x_sq, 2.0);
        } else {
            const double denom = static_cast<double>(2 * k) * static_cast<double>(2 * k - 1);
            term = asm_mul(term, asm_div(-two_x_sq, denom));
        }

        sum = asm_add(sum, term);
        if (n_eps == 0 && std::fabs(y - sum) < eps) {
            n_eps = k;
        }
    }

    return Result{x, y, sum, n_eps};
}

std::vector<double> make_points(double a, double b, double h) {
    std::vector<double> xs;
    for (double x = a; x <= b + h * 0.5; x += h) {
        xs.push_back(x);
    }
    return xs;
}

} // namespace

int main(int argc, char** argv) {
    try {
        const Config cfg = parse_args(argc, argv);
        const std::vector<double> xs = make_points(cfg.a, cfg.b, cfg.h);

        std::cout << "Single-thread CPU run\n";
        std::cout << std::setw(12) << "x"
                  << std::setw(18) << "Y(x)"
                  << std::setw(18) << "S(x)"
                  << std::setw(12) << "n" << "\n";
        std::cout << std::string(60, '-') << "\n";
        for (double x : xs) {
            const Result row = calculate_series(x, cfg.terms, cfg.eps);
            std::cout << std::fixed << std::setprecision(8)
                      << std::setw(12) << row.x
                      << std::setw(18) << row.y
                      << std::setw(18) << row.s
                      << std::setw(12) << row.n_eps << "\n";
        }

        std::ofstream csv(cfg.out);
        if (!csv) {
            throw std::runtime_error("cannot open output CSV");
        }
        csv << "elapsed_seconds,iterations,checksum\n";

        double checksum = 0.0;
        const auto start = std::chrono::steady_clock::now();
        for (int done = 0; done < cfg.iterations; done += cfg.checkpoint) {
            const int batch = std::min(cfg.checkpoint, cfg.iterations - done);
            for (int i = 0; i < batch; ++i) {
                const double x = xs[static_cast<std::size_t>(done + i) % xs.size()];
                checksum += calculate_series(x, cfg.terms, cfg.eps).s;
            }

            const auto now = std::chrono::steady_clock::now();
            const std::chrono::duration<double> elapsed = now - start;
            csv << elapsed.count() << ',' << (done + batch) << ',' << checksum << '\n';
        }

        std::cout << "\nCSV written to " << cfg.out << "\n";
        return 0;
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
}
