using Laboratory4.AbstractFactory;
using Laboratory4.Export;
using Laboratory4.Export.Decorator;
using Laboratory4.Export.Serializer;
using Laboratory4.Strategy;
using Laboratory4.TypeTransport;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Laboratory4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MySerializers mySerializer = new MySerializers();
            Deserialize readDelivery = new Deserialize();
            var delivery = readDelivery.MyJsonDeserialize();


            if (delivery.realtransports[0].Type == "")
            {
                Console.WriteLine("Не был указан транспорт" +
                                  "\nСтоимость и время для вашей доставки каждого транспорта");
                delivery.ShowAllInfo(delivery);
                return;
            }
            delivery.SetResult();
            delivery.ShowInfo();


        
            //ISortCriterion<Result> byPriceAsc = new SortByPriceCriterion(SortDirection.Ascending);
            //ISortCriterion<Result> bySpeedDesc = new SortBySpeedCriterion(SortDirection.Descending);
            //var sortPipeline = new SortPipeline<Result>();
            //sortPipeline.AddCriterion(byPriceAsc);   
            //sortPipeline.AddCriterion(bySpeedDesc);  
            //var finalList = sortPipeline.Apply(delivery.results).ToList();
            //delivery.results = finalList;
            //delivery.ShowInfo();





            string json = mySerializer.MyJsonSerializer(delivery);
            string csv = mySerializer.MyCsvSerializer(delivery);

            //var saver = new ZipSaver(new JsonSaver());
            //saver.SaveToZip(json, "data.zip", "data.json");

            //var saver = new ZipSaver(
            //                new EncryptSaver(
            //                    new JsonSaver(),
            //                    "MyPassword"));
            //saver.SaveToZip(json, "secret.zip", "secret.json");


            //var saver = new ZipSaver(
            //        new CsvSaver()
            //      );
            //saver.SaveToZip(csv, "report.zip", "report.csv");

        }
    }
}
