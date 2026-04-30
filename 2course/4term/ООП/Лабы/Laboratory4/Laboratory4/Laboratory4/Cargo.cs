using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4
{
    public class Cargo
    {
        public Product product;
        private double quantity;
        public double Quantity
        {
            get => quantity; 
            set => quantity = value;
        }

        public Cargo(Product product, double quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
        public double getCost()
        {
            return quantity * product.UnitPerKillo * product.CostPerKillo;
        }
    }
}
