using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Insurance
    {
        private string type;
        private double cost;
        public string Type
        {
            get => type;

            set
            {
                type = value;
            }
        }
        public double Cost
        {
            get => cost;

            set
            {
                cost = value;
            }
        }

        public Insurance(string type, double cost)
        {
            Type = type;
            Cost = cost;
        }
    }

}
