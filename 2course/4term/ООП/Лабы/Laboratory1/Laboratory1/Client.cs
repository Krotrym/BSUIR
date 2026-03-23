using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Laboratory1
{
    public class Client
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;

        public string FirstName
        {
            get => firstName;
            set { firstName = value; }
        }

        public string LastName
        {
            get => lastName;
            set { lastName = value; }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; }
        }

        public Client()
        {
            firstName = "";
            lastName = "";
            phoneNumber = "";
        }

        public void SetClientData(string fName, string lName, string phone)
        {
            firstName = fName;
            lastName = lName;
            phoneNumber = phone;
        }

        public string GetClientInfo()
        {
            return "Имя: " + firstName + " " + lastName + ", Тел: " + phoneNumber;
        }
    }

}
