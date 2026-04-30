using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankCreditAccount : BankAccount
    {
        private double ammountCredit;
        private Loan loan = new Loan();
        public double AmountCredit
        {
            get => ammountCredit; 
            set => ammountCredit = value;
        }
        public BankCreditAccount(string email, string password, string currency, double ammountCredit, string id)
            : base(email, password, currency, id)
        {
            this.ammountCredit = ammountCredit;
        }

        public override void Deposit(double amount)
        {
            if(amount > 0)
            Balance += amount;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Email:{Email} Pasword:{Password} Balance:{Balance} Currency:{Currency} AmountCredit{ammountCredit}");
        }
        public override void Withdraw(double amount)
        {
            if(amount >0 && Balance > amount)
                Balance -= amount;
        }

        public override string GetAccountType()
        {
            return "Кредитный";
        }

        public void PayCredit(double amount)
        {
            if(amount > 0 && Balance > amount && ammountCredit > amount)
            {
                ammountCredit -= amount;
                Withdraw(amount);
            }
        }
    }
}
