using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class Card
    {
        private string cardNumber;
        private string pinCode;
        private string expiryDate;
        private bool isBlocked;

        public string CardNumber
        {
            get => cardNumber;
            set { cardNumber = value; }
        }

        public string PinCode
        {
            get => pinCode;
            set { pinCode = value; }
        }

        public string ExpiryDate
        {
            get => expiryDate;
            set { expiryDate = value; }
        }

        public bool IsBlocked
        {
            get => isBlocked;
            set { isBlocked = value; }
        }

        public Card()
        {
            cardNumber = "";
            pinCode = "0000";
            expiryDate = "12/30";
            isBlocked = false;
        }

        public bool CheckPin(string enteredPin)
        {
            return enteredPin == pinCode;
        }

        public void Block()
        {
            isBlocked = true;
            Console.WriteLine("Карта " + cardNumber + " заблокирована");
        }

        public void Unblock()
        {
            isBlocked = false;
            Console.WriteLine("Карта " + cardNumber + " разблокирована");
        }
    }
}
