using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Loan
    {
        private double amount;
        private double interestRate;
        private int months;
        private double monthlyPayment;

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

        public int Months
        {
            get => months;
            set { months = value; }
        }

        public double MonthlyPayment
        {
            get => monthlyPayment;
            set { monthlyPayment = value; }
        }

        public Loan()
        {
            amount = 0;
            interestRate = 0;
            months = 0;
            monthlyPayment = 0;
        }

        public void CalculateMonthlyPayment()
        {
            if (interestRate == 0 || months == 0)
            {
                monthlyPayment = amount / months;
                return;
            }
            double monthlyRate = interestRate / 12 / 100;
            monthlyPayment = amount * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -months));
        }

        public void ShowLoanInfo()
        {
            Console.WriteLine("Cумма " + amount + ", ставка " + interestRate + "%, срок " + months + " мес.");
            Console.WriteLine("Ежемесячный платеж: " + monthlyPayment);
        }
    }

}
