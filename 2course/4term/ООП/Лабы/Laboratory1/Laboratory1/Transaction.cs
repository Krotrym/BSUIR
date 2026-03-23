using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Transaction
    {
        private DateTime date;
        private double amount;
        private string type;
        private string status;

        public DateTime Date
        {
            get => date;
            set { date = value; }
        }

        public double Amount
        {
            get => amount;
            set { amount = value; }
        }

        public string Type
        {
            get => type;
            set { type = value; }
        }

        public string Status
        {
            get => status;
            set { status = value; }
        }

        public Transaction()
        {
            date = DateTime.Now;
            amount = 0;
            type = "";
            status = "Pending";
        }

        public void Execute()
        {
            if (amount > 0)
            {
                status = "Completed";
                Console.WriteLine("Транзакция  выполнена на сумму " + amount);
            }
            else
            {
                status = "Failed";
                Console.WriteLine("Ошибка выполнения транзакции");
            }
        }

        public void Cancel()
        {
            if (status == "Completed")
            {
                status = "Cancelled";
                Console.WriteLine("Транзакция  отменена");
            }
        }
    }

}
