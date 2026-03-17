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
        public double Amount
        {
            get => amount;

            set
            {
                amount = value;
            }
        }
        public double InterestRate
        {
            get => interestRate;

            set
            {
                interestRate = value;
            }
        }
        public int Months
        {
            get => months;

            set
            {
                months = value;
            }
        }

        public Loan(double amount, double interestRate, int months)
        {
            Amount = amount;
            InterestRate = interestRate;
            Months = months;
        }

        public double CalculateMonthlyPayment()
        {
            double monthlyRate = InterestRate / 12 / 100;
            return Amount * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -Months));
        }
    }

}
