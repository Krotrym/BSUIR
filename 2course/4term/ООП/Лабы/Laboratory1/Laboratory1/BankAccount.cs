using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankAccount
    {
        private string accountNumber;
        private string currency;
        private double balance;

        public string AccountNumber
        {
            get => accountNumber;
            set
            {
                accountNumber = value;
            }
        }
        public string Currency
        {
            get => currency;
            set
            {
                currency = value;
            }
        }
        public double Balance
        {
            get => balance;
            set
            {
                balance = value;
            }
        }

        public BankAccount(string accountNumber, string currency)
        {
            AccountNumber = accountNumber;
            Currency = currency;
            Balance = 0.0;
        }

        public void Deposit(double amount)
        {
            Balance += amount;
        }

        public bool Withdraw(double amount)
        {
            if (amount > Balance) return false;
            Balance -= amount;
            return true;
        }
    }

}
