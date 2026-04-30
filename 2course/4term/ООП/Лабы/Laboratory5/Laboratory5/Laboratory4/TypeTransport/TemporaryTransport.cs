using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.TypeTransport
{
    public class TemporaryTransport
    {
        private double consumption_per_kilometer;
        private double speed;
        private string type;
        private string factory;
        public string Factory
        {
            get => factory;
            set => factory = value;
        }
        public double ConsumptionPerkilometer
        {
            get => consumption_per_kilometer;
            set => consumption_per_kilometer = value;
        }

        public double Speed
        {
            get => speed;
            set => speed = value;
        }

        public string Type
        {
            get => type;
            set => type = value;
        }
        public TemporaryTransport() { }
        
    }
}
