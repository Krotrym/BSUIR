using Laboratory4.TypeTransport;
using Laboratory4.Export.Serializer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Laboratory4
{
    public class Delivery
    {
        public CargoBatch cargo { get; set; }
        public List<TemporaryTransport> transports { get; set; }
        public List<Transport> realtransports { get; set; } = new List<Transport>();
        public List<Result> results { get; set; } = new List<Result>();
        public double distance { get; set; }

        public Delivery() { }

        public double CalculateCost(Transport transport)
        {
            return cargo.GetTotalCargoCost() + distance * transport.ConsumptionPerkilometer;
        }

        public double CalculateTime(Transport transport)
        {
            return distance / transport.Speed;
        }

        public void SetResult()
        {
            foreach (var item in realtransports)
            {
            results.Add(new Result(item.Type, CalculateCost(item), CalculateTime(item)));
            }
        }
        public void ShowInfo()
        {
            foreach (var item in results)
            {
                Console.WriteLine($"Доставка с помощью {item.Type}");
                Console.WriteLine($"Стоимость: {item.Cost}$");
                Console.WriteLine($"Время: {item.Time}ч");
                Console.WriteLine();
            }
        }
        public void ShowAllInfo(Delivery delivery)
        {
            Deserialize deserialize = new Deserialize();
            string json = File.ReadAllText("D:\\Study\\2course\\4term\\ООП\\Лабы\\Laboratory5\\Laboratory5\\Laboratory4\\Info.json");
            Root root = JsonSerializer.Deserialize<Root>(json);
            delivery.transports = root.transports;
            delivery = deserialize.AddNormalTransport(delivery);
            foreach (var item in delivery.realtransports)
            {
                Console.WriteLine($"Тип транспорта: {item.Type}");
                Console.WriteLine($"Стоимость доставки: {delivery.CalculateCost(item)}$");
                Console.WriteLine($"Время доставки: {delivery.CalculateTime(item)}ч");
                Console.WriteLine();
            }
        }
    }
}
