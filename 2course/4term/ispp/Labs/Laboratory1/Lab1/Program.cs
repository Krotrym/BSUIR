using System;
using System.Diagnostics;
using System.Threading;


namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество одновременно выполняющихся потоков ");
            int quantity = Int32.Parse(Console.ReadLine());

            CalculateInt calculateInt = new CalculateInt(quantity);
            Program program = new Program();
            calculateInt.Calculate += program.WriteTime;
            calculateInt.Progress += program.WritePercent;


            Thread thread1 = new Thread(calculateInt.CalculateIntegral);
            Thread thread2 = new Thread(calculateInt.CalculateIntegral);
            Thread thread3 = new Thread(calculateInt.CalculateIntegral);
            Thread thread4 = new Thread(calculateInt.CalculateIntegral);
            Thread thread5 = new Thread(calculateInt.CalculateIntegral);

            thread1.Priority = ThreadPriority.Highest;
            thread2.Priority = ThreadPriority.Lowest;
            thread3.Priority = ThreadPriority.Lowest;
            thread4.Priority = ThreadPriority.Lowest;
            thread5.Priority = ThreadPriority.Highest;

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();    
        }
        public void WriteTime(long time)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Поток {threadId}:Завершён с результат {time}");
        }

        public void WritePercent(long time)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;            
                Console.WriteLine($"Поток {threadId}:[=========>]{time}%");
        }
    }
}
