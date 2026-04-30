using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Atm
    {
        private string location;
        private double cashBalance;
        private bool isOnline;

        public string Location
        {
            get => location; 
            set { location = value; }
        }

        public double CashBalance
        {
            get => cashBalance; 
            set { cashBalance = value; }
        }

        public bool IsOnline
        {
            get => isOnline; 
            set { isOnline = value; }
        }

        public Atm()
        {
            location = "";
            cashBalance = 0;
            isOnline = false;
        }

        public bool WithdrawCash(double amount)
        {
            if (isOnline && amount > 0 && amount <= cashBalance)
            {
                cashBalance = cashBalance - amount;
                Console.WriteLine("ATM выдал " + amount);
                return true;
            }
            Console.WriteLine("Операция недоступна");
            return false;
        }

        public void LoadCash(double amount)
        {
            if (amount > 0)
            {
                cashBalance = cashBalance + amount;
                Console.WriteLine("ATM загружен на " + amount);
            }
        }
    }

}
