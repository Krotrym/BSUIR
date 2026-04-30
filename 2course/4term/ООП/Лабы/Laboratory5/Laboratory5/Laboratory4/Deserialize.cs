using Laboratory4.AbstractFactory;
using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Laboratory4
{
    public class Root
    {
        public List<Product> products { get; set; }
        public List<TemporaryTransport> transports { get; set; }
    }
    internal class Deserialize
    {
        public Delivery AddNormalTransport(Delivery delivery)
        {
            foreach (var item in delivery.transports)
            {
                IAbstractFactory abstractFactory = GetFactory(item);
                delivery.realtransports.Add(abstractFactory.CreateTransport(item.ConsumptionPerkilometer, item.Speed, item.Type));
            }
            return delivery;
        }

        public IAbstractFactory GetFactory(TemporaryTransport temporaryTransport)
        {
            return temporaryTransport.Factory switch
            {
                "Ground" => new GroundFactory(),
                "Air" => new AirFactory(),
                "Water" => new WaterFactory()
            };
        }


        public Delivery MyJsonDeserialize()
        {
            string json = File.ReadAllText("D:\\Study\\2course\\4term\\ООП\\Лабы\\Laboratory5\\Laboratory5\\Laboratory4\\name.json");
            Delivery delivery = new Delivery();
            delivery = JsonSerializer.Deserialize<Delivery>(json);
            delivery = AddNormalTransport(delivery);
            return delivery;
        }

        public Delivery MyXmlDeserialize()
        {
            var deserializer = new XmlSerializer(typeof(Delivery));
            using var stream = File.OpenRead("D:\\Study\\2course\\4term\\ООП\\Лабы\\Laboratory5\\Laboratory5\\Laboratory4\\name.xml");
            Delivery delivery = new Delivery();
            delivery = (Delivery)deserializer.Deserialize(stream);
            delivery = AddNormalTransport(delivery);
            return delivery;
        }
        //public Delivery MyCsvDeserializer()
        //{

        //}
    }
}
