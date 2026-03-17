using System.Data;

namespace APP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("Aboba", "23", "+375");
            string avova = client.FullName;
            Console.WriteLine(avova);
        }
    }
}
