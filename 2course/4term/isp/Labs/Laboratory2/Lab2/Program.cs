using LibraryLab2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace Lab2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Stream stream = new MemoryStream();
            StreamService<CharacterOfGame> service = new StreamService<CharacterOfGame>();
            Program program = new Program();
            service.EndWriteOrRead += program.MessageEndMethod;


            var progress = new Progress<string>(message =>
            {
                Console.WriteLine($"{message}");
            });

            Console.WriteLine("Введите имя файла .txt в который запишется колекция(Путь этого файла будет: 'D:\\2 курс\\4 семестр\\ИСП\\Лабы\\LibraryLab2\\Lab2\\bin\\Debug\\net8.0' ");
            string? fileName = Console.ReadLine();

            List<CharacterOfGame> list = new List<CharacterOfGame>();

            for (int i = 0; i < 1000; i++)
            {
                list.Add(new CharacterOfGame(i, "SuperMan", true));
            }

            var thread1 = Task.Run(() =>
            
                service.WriteToStreamAsync(stream, list, progress));
            Thread.Sleep(100);

            var thread2 = Task.Run(() =>
            
                service.CopyFromStreamAsync(stream, fileName, progress).Wait());

            //thread1.Start();




            //thread2.Start();
            //Console.WriteLine($"Поток {thread2.ManagedThreadId} начал выполнение");

            //thread1.Join();
            //thread2.Join();
            await Task.WhenAll(thread1, thread2);

            int quantity = await service.GetStatisticsAsync(fileName, x => x.Name == "SuperMan");
            Console.WriteLine($"Количесвто игровых персоонажей в коллекции с именем SuperMan: {quantity}");
        }
        private void MessageEndMethod(string message)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(  );
        }
    }
}
