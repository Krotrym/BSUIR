using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Export.Serializer
{
    public class InformationForExport
    {
        private string type;
        private double cost;
        private double time;

        public string Type { get => type; set => type = value; }
        public double Cost { get => cost; set => cost = value; }
        public double Time { get => time; set => time = value; }
    }
}
