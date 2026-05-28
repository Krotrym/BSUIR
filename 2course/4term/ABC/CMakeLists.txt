cmake_minimum_required(VERSION 3.16)

project(ABC_Labs_2_3 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

add_executable(lab1_single lab1_single.cpp)

find_package(OpenMP REQUIRED)

add_executable(lab2_openmp lab2_openmp.cpp)
target_link_libraries(lab2_openmp PRIVATE OpenMP::OpenMP_CXX)

find_package(OpenCL)
if(OpenCL_FOUND)
    add_executable(lab3_opencl lab3_opencl.cpp)
    target_link_libraries(lab3_opencl PRIVATE OpenCL::OpenCL)
else()
    message(WARNING "OpenCL was not found. lab3_opencl will not be built.")
endif()
