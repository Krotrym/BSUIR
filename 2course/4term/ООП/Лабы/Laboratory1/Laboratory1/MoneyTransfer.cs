using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class MoneyTransfer
    {
        private string fromAccount;
        private string toAccount;
        private double amount;
        private double commission;
        private string status;

        public string FromAccount
        {
            get => fromAccount;
            set { fromAccount = value; }
        }

        public string ToAccount
        {
            get => toAccount;
            set { toAccount = value; }
        }

        public double Amount
        {
            get => amount;
            set { amount = value; }
        }

        public double Commission
        {
            get => commission;
            set { commission = value; }
        }

        public string Status
        {
            get => status;
            set { status = value; }
        }

        public MoneyTransfer()
        {
            fromAccount = "";
            toAccount = "";
            amount = 0;
            commission = 0;
            status = "New";
        }

        public void CalculateCommission(double percent)
        {
            commission = amount * percent / 100;
        }

        public void Execute()
        {
            if (amount > 0 && fromAccount != "" && toAccount != "")
            {
                status = "Executed";
                Console.WriteLine("Перевод выполнен: " + amount + " (комиссия " + commission + ")");
            }
        }
    }
}
