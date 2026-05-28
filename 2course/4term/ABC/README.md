# Лабораторные 1, 2 и 3 по АВС

Вариант из отчёта:  
`S(x) = sum(k=1..n) (-1)^k * (2x)^(2k) / (2k)!`  
`Y(x) = 2(cos^2(x) - 1)`.

## Суть задания

Лабораторная 1 проверяет выполнение вычислений на CPU в однопоточном режиме. Программа считает ряд для заданного диапазона `x`, использует x87/FPU-ассемблерные вставки и пишет CSV для графика `elapsed_seconds -> iterations`. Для демонстрации нагрузки на одно ядро процессорный поток лучше закрепить через `taskset`.

Лабораторная 2 проверяет, как та же вычислительная задача работает на всех ядрах CPU. Программа считает ряд для заданного диапазона `x`, использует вставки x87/FPU-ассемблера для базовых операций и распараллеливает независимые вычисления через OpenMP. Её нужно запускать два раза: с Hyper-Threading/SMT и без него, затем сравнить CSV-графики `elapsed_seconds -> iterations`.

Лабораторная 3 переносит массовые вычисления той же функции на GPU/iGPU. Программа запускает OpenCL-ядро, где каждый work-item считает ряд для одного `x`. Результат также пишется в CSV для графика `elapsed_seconds -> iterations`.

## Сборка в Ubuntu/Debian

```bash
sudo apt update
sudo apt install build-essential cmake libomp-dev ocl-icd-opencl-dev clinfo
```

Если OpenCL-рантайм для видеокарты не установлен, поставь пакет от своего вендора:
Intel GPU обычно требует `intel-opencl-icd`, NVIDIA - драйвер NVIDIA/OpenCL, AMD - ROCm или Mesa OpenCL.

```bash
cmake -S . -B build -DCMAKE_BUILD_TYPE=Release
cmake --build build -j
```

## Перед показом

В одном терминале запускай программу, во втором открывай мониторинг:

```bash
htop
```

Для GPU на Intel:

```bash
sudo intel-gpu-top
```

Для NVIDIA:

```bash
watch -n 0.5 nvidia-smi
```

Проверить количество потоков на ядро:

```bash
lscpu | grep -E 'CPU|Thread|Core|Socket|Model name'
```

Быстро отключить SMT/Hyper-Threading до перезагрузки, если система разрешает:

```bash
cat /sys/devices/system/cpu/smt/control
echo off | sudo tee /sys/devices/system/cpu/smt/control
```

Вернуть обратно:

```bash
echo on | sudo tee /sys/devices/system/cpu/smt/control
```

Если временное отключение не работает, добавляют `nosmt` в параметры ядра через `/etc/default/grub`, выполняют `sudo update-grub` и перезагружаются.

## Запуск ПЗ 1

Однопоточный запуск с закреплением на одном логическом ядре:

```bash
taskset -c 0 ./build/lab1_single --a -1 --b 1 --h 0.1 --eps 1e-8 --terms 10000 --iterations 20000 --out lab1_single.csv
```

Что показывать: в `htop` будет заметна нагрузка в основном на одном логическом ядре. CSV `lab1_single.csv` нужен для графика.

## Запуск ПЗ 2

```bash
OMP_NUM_THREADS=$(nproc) ./build/lab2_openmp --a -1 --b 1 --h 0.1 --eps 1e-8 --terms 10000 --iterations 20000 --out lab2_ht_on.csv
```

Для опыта без HT/SMT в Linux обычно добавляют параметр ядра `nosmt`, перезагружаются и запускают ту же команду:

```bash
OMP_NUM_THREADS=$(nproc) ./build/lab2_openmp --a -1 --b 1 --h 0.1 --eps 1e-8 --terms 10000 --iterations 20000 --out lab2_ht_off.csv
```

Что показывать: в `htop` должны быть загружены все доступные ядра/потоки. Для отчёта сравниваются `lab2_ht_on.csv` и `lab2_ht_off.csv`.

## Запуск ПЗ 3

Сначала можно посмотреть доступные OpenCL-устройства:

```bash
clinfo | less
```

Обычный запуск:

```bash
./build/lab3_opencl --a -1 --b 1 --h 0.1 --eps 1e-8 --terms 10000 --iterations 20000 --batch 1024 --out lab3_gpu.csv
```

Если устройств несколько, выбери нужные индексы:

```bash
./build/lab3_opencl --platform 0 --device 0 --out lab3_gpu.csv
```

Что показывать: в `intel-gpu-top`, `nvidia-smi` или другом GPU-мониторе должна появиться нагрузка GPU/iGPU. CPU при этом занят в основном запуском OpenCL-ядра и чтением результата.

CSV-файлы открываются в LibreOffice Calc, Excel или строятся любым Python/gnuplot-скриптом. Для отчёта нужны столбцы `elapsed_seconds` по оси X и `iterations` по оси Y.

## Построение графиков

Без дополнительных библиотек можно сделать SVG-график:

```bash
python3 plot_csv_svg.py lab1_single.csv -o lab1_single.svg --title "График выполнения в одноядерном режиме"
python3 plot_csv_svg.py lab2_2threads.csv lab2_4threads.csv -o lab2_compare.svg --title "График выполнения в многоядерном режиме"
```

SVG открывается в браузере и вставляется в отчёт как обычная картинка. Если нужен PNG, открой SVG в браузере и сделай скриншот нужной области.
