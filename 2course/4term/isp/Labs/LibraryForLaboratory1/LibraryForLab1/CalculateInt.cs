using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class CalculateInt
    {
        SemaphoreSlim semaphore;
        public CalculateInt(int quantity)
        {
            semaphore = new SemaphoreSlim(quantity);
        }

        public event Action<long>? Progress;
        public event Action<long>? Calculate;

        //private static object doorLock = new object();    

        public void CalculateIntegral()
        {
            //lock (doorLock)
            //{
            semaphore.Wait();
            Stopwatch stopWatch = Stopwatch.StartNew();

            double h = 0.00000001;
            double integral = 0.0;
            double n = 1 / h;
            double progressStep = n / 100;

            int delay = 0;

            for (long i = 0; i < 100000000 - 1; i++)
            {
                integral += h * Math.Sin(h * (i + 0.5));
                for (long j = 0; j < 100; j++) //j = 100000
                {
                    delay = 2 + 2;
                }
                if (i % progressStep == 0)
                {
                    int percent = (int)((double)i / n * 100);
                    Progress?.Invoke(percent);
                }
            }
            stopWatch.Stop();
            semaphore.Release();
            Calculate?.Invoke(stopWatch.ElapsedTicks);
            //}
        }
    }
}
