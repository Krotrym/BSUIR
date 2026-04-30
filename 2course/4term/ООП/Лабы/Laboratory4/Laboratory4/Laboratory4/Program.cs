using Laboratory4.AbstractFactory;
using Laboratory4.TypeTransport;

namespace Laboratory4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var electronic = new Product("Электроника", 1.5, 50);
            var clothes = new Product("Одежд", 0.8, 20);
            var equipment = new Product("Оборудовани", 120, 15);
            var perishableProducts = new Product("Скоропортящиеся продукт", 10, 100);

            var batch1 = new CargoBatch();
            batch1.AddLine(electronic, 10);
            batch1.AddLine(clothes, 25);

            var batch2 = new CargoBatch();
            batch2.AddLine(equipment, 1);
            batch2.AddLine(perishableProducts, 50);

            IAbstractFactory factory1 = new AirFactory();
            Transport transport1 = factory1.CreateTransport(150, 850, "Воздушный");

            IAbstractFactory factory2 = new WaterFactory();
            Transport transport2 = factory2.CreateTransport(2, 35, "Вода");

            var delivery1 = new Delivery(batch1, transport1, 1200);
            var delivery2 = new Delivery(batch2, transport2, 2400);


            Console.WriteLine("Доставка номер 1");
            Console.WriteLine($"Итоговая стоимость: {delivery1.CalculateCost()}$");
            Console.WriteLine($"Время доставки: {delivery1.CalculateTime()}ч");

            Console.WriteLine();

            Console.WriteLine("Доставка номер 2");
            Console.WriteLine($"Итоговая стоимость: {delivery2.CalculateCost()}$");
            Console.WriteLine($"Время доставки: {delivery2.CalculateTime()}ч");
        }
    }
}
