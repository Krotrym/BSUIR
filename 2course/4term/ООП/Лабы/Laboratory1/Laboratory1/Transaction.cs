using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Transaction
    {
        private string type;
        private double amount;
        private string date;
        public string Type
        {
            get => type;

            set
            {
                type = value;
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
        public string Date
        {
            get => date;

            set
            {
                date = value;
            }
        }

        public Transaction(string type, double amount, string date)
        {
            Type = type;
            Amount = amount;
            Date = date;
        }
    }

}
