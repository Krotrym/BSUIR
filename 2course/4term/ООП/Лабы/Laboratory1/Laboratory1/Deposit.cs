using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Deposit
    {
        private double amount;
        private double interestRate;
        private int termMonths;
        private DateTime startDate;

        public double Amount
        {
            get => amount;
            set { amount = value; }
        }

        public double InterestRate
        {
            get => interestRate;
            set { interestRate = value; }
        }

        public int TermMonths
        {
            get => termMonths;
            set { termMonths = value; }
        }

        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; }
        }

        public Deposit()
        {
            amount = 0;
            interestRate = 0;
            termMonths = 0;
            startDate = DateTime.Now;
        }

        public double CalculateProfit()
        {
            double monthlyRate = interestRate / 12 / 100;
            return amount * monthlyRate * termMonths;
        }

        public void ShowInfo()
        {
            Console.WriteLine("Сумма:" + amount + ", ставка " + interestRate + "%, срок " + termMonths + " мес.");
            Console.WriteLine("Доход по окончании: " + CalculateProfit());
        }
    }
}
