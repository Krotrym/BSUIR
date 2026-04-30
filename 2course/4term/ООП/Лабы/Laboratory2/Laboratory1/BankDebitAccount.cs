using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankDebitAccount : BankAccount
    {
        public BankDebitAccount(string email, string password, string currency, string id)
           : base(email, password, currency, id)
        {
        }

        public override void Deposit(double amount)
        {
            if (amount > 0)
                Balance += amount;
        }
        public override void ShowInfo()
        {
            Console.WriteLine($"Email:{Email} Pasword:{Password} Balance:{Balance} Currency:{Currency}");
        }
        public override void Withdraw(double amount)
        {
            if (amount > 0 && Balance > amount)
                Balance -= amount;
        }
        public override string GetAccountType()
        {
            return "Накопительный";
        }
    }
}
