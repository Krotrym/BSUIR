using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Investment
    {
        private string assetType;
        private double amount;
        public string AssetType
        {
            get => assetType;

            set
            {
                assetType = value;
            }
        }
        public double Amount
        {
            get => amount;

            set
            {
                amount = value;
            }
        }

        public Investment(string assetType, double amount)
        {
            AssetType = assetType;
            Amount = amount;
        }

        public void Increase(double value)
        {
            Amount += value;
        }
    }

}
