using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public abstract class BankAccount
    {
        private string email;
        private string password;
        private double balance;
        private string currency;
        public string idAccount;
        public string Email
        {
            get => email;
            set { email = value; }
        }

        public string Password
        {
            get => password;
            set { password = value; }
        }
        public double Balance
        {
            get => balance;
            set { balance = value; }
        }

        public string Currency
        {
            get => currency;
            set { currency = value; }
        }

        public BankAccount(string email, string password, string currency, string idAccount)
        {
            Email = email;
            Password = password;
            Currency = currency;
            balance = 0;
            this.idAccount = idAccount;
        }

        public abstract void Deposit(double amount);
        public abstract void Withdraw(double amount);
        public abstract void ShowInfo();
        public abstract string GetAccountType();
    }

}
