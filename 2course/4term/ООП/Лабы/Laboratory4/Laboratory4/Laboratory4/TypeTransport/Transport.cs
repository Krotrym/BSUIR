using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Laboratory4.TypeTransport
{
    internal abstract class Transport
    {
        private double consumption_per_kilometer;
        private double speed;
        private string type;

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

    }
}
