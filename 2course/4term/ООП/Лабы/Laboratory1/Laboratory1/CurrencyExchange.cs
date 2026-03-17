using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class CurrencyExchange
    {
        private string fromCurrency;
        private string toCurrency;
        private double rate;
        public string FromCurrency
        {
            get => fromCurrency;

            set
            {
                fromCurrency = value;
            }
        }
        public string ToCurrency
        {
            get => toCurrency;

            set
            {
                toCurrency = value;
            }
        }
        public double Rate
        {
            get => rate;

            set
            {
                rate = value;
            }
        }

        public CurrencyExchange(string fromCurrency, string toCurrency, double rate)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Rate = rate;
        }

        public double Convert(double amount)
        {
            return amount * Rate;
        }
    }

}
