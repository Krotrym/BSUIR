using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4
{
    public class CargoBatch
    {
        public List<Cargo> lines = new List<Cargo>();

        public void AddLine(Product product, int quantity)
        {
            lines.Add(new Cargo(product, quantity));
        }
        public double GetTotalCargoCost()
        {
            return lines.Sum(line => line.getCost());
        }
    }

}
