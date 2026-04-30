using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4
{
    public class Product
    {
        private string name;
        public string Name 
        { 
            get => name; 
            set => name = value; 
        }

        private double unit_per_killo;
        public double UnitPerKillo
        {
            get => unit_per_killo;
            set => unit_per_killo = value;
        }

        private double cost_per_killo;
        public double CostPerKillo
        {
            get => cost_per_killo;
            set => cost_per_killo = value;
        }
        public Product(string name, double unit_per_killo, double cost_per_killo)
        {
            Name = name;
            UnitPerKillo = unit_per_killo;
            CostPerKillo = cost_per_killo;
        }
    }
}
