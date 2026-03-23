using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class ExchangeRate
    {
        private string currencyFrom;
        private string currencyTo;
        private double rate;

        public string CurrencyFrom
        {
            get => currencyFrom;
            set { currencyFrom = value; }
        }

        public string CurrencyTo
        {
            get => currencyTo;
            set { currencyTo = value; }
        }

        public double Rate
        {
            get => rate;
            set { rate = value; }
        }

        public ExchangeRate()
        {
            currencyFrom = "USD";
            currencyTo = "RUB";
            rate = 0;
        }

        public double Convert(double amount)
        {
            return amount * rate;
        }

        public void UpdateRate(double newRate)
        {
            rate = newRate;
            Console.WriteLine("Курс обновлен: 1 " + currencyFrom + " = " + rate + " " + currencyTo);
        }
    }
}
