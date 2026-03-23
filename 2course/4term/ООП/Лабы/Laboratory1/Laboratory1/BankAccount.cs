using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankAccount
    {
        private string accountNumber;
        private double balance;
        private string currency;

        public string AccountNumber
        {
            get => accountNumber;
            set { accountNumber = value; }
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

        public BankAccount()
        {
            accountNumber = "";
            balance = 0;
            currency = "RUB";
        }

        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                balance = balance + amount;
                Console.WriteLine("Пополнение: " + amount + " " + currency);
            }
        }

        public bool Withdraw(double amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance = balance - amount;
                Console.WriteLine("Снятие: " + amount + " " + currency);
                return true;
            }
            Console.WriteLine("Недостаточно средств!");
            return false;
        }

        public void ShowBalance()
        {
            Console.WriteLine("Счет: " + accountNumber + ", Баланс: " + balance + " " + currency);
        }
    }

}
