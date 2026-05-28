using System.Collections.Generic;
using System.Text.Json;

namespace LibraryLab2
{
    public class StreamService<T>
    {
        public event Action<string> EndWriteOrRead;
        private int Calculate = 0;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public async Task WriteToStreamAsync(Stream stream, IEnumerable<CharacterOfGame> data, IProgress<string> progress)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начал выполнение");
            semaphore?.Wait();
            progress?.Report($"Поток - {Thread.CurrentThread.ManagedThreadId}, зашёл в метод WriteToStreamAsync");
            using (StreamWriter writer = new StreamWriter(stream, leaveOpen: true))
            {
                progress?.Report($"Поток - {Thread.CurrentThread.ManagedThreadId}, начинает получать коллекцию");
                foreach (var item in data)
                {
                    Calculate++;
                    if (Calculate % 100 == 0)
                    {
                        progress?.Report($"Записано в поток {Calculate} из 1000 элементов");
                    }
                    string json = JsonSerializer.Serialize(item);
                    await writer.WriteLineAsync(json);
                    Thread.Sleep(1);
                }
                await writer.FlushAsync();
                Calculate = 0;
            }
            progress?.Report($"Поток {Thread.CurrentThread.ManagedThreadId} получил всю колекцию");
            semaphore?.Release();
            stream.Position = 0;
        }

        public async Task CopyFromStreamAsync(Stream stream, string fileName, IProgress<string> progress)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начал выполнение");
            progress?.Report($"Поток - {Thread.CurrentThread.ManagedThreadId} вошёл в метод CopyFromStreamAsync");
            semaphore?.Wait();
            using (FileStream fileStream = File.Create(fileName))
            {
                progress?.Report($"Поток - {Thread.CurrentThread.ManagedThreadId} начал записывать колекцию в файл");
                await stream.CopyToAsync (fileStream);
            }
            progress?.Report($"Поток - {Thread.CurrentThread.ManagedThreadId} закончил записывать колекцию в файл");
            semaphore?.Release();
        }

        public async Task<int> GetStatisticsAsync(string fileName, Func<CharacterOfGame, bool> filter)
        {
            int count = 0;
            string line = "";
            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var item = JsonSerializer.Deserialize<CharacterOfGame>(line);

                    if (item != null && filter(item))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }
}
