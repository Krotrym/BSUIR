using Laboratory4.TypeTransport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Laboratory4.Export.Serializer
{
    public class MySerializers
    {
        public string MyJsonSerializer(Delivery delivery)
        {
            var list = new List<InformationForExport>();

            foreach (var item in delivery.realtransports)
            {
                list.Add(new InformationForExport
                {
                    Type = item.Type,
                    Cost = delivery.CalculateCost(item),
                    Time = delivery.CalculateTime(item)
                });
            }

            //Без этого русские буквы будут в utf коде Бог знает почему
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            //
            return JsonSerializer.Serialize(list, options);
        }

        public string MyCsvSerializer(Delivery delivery)
        {
            var lines = new List<string>();

            lines.Add("Type,Cost,Time");

            foreach (var item in delivery.realtransports)
            {
                var info = new InformationForExport
                {
                    Type = item.Type,
                    Cost = delivery.CalculateCost(item),
                    Time = delivery.CalculateTime(item)
                };

                string line = $"{info.Type},{info.Cost},{info.Time}";
                lines.Add(line);
            }
            return string.Join("\n", lines);
        }
    }
}
