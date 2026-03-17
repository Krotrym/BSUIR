using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class ATM
    {
        private string location;
        private double cashAmount;
        public string Location
        { get => location;
            set
            {
                location = value;
            }
        }

        public double CashAmount
        {
            get => cashAmount;
            set
            {
                cashAmount = value;
            }
        }

        public ATM(string location, double cashAmount)
        {
            Location = location;
            CashAmount = cashAmount;
        }

        public bool WithdrawCash(double amount)
        {
            if (amount > CashAmount) return false;
            CashAmount -= amount;
            return true;
        }

        public void LoadCash(double amount)
        {
            CashAmount += amount;
        }
    }

}
