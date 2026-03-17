using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Laboratory1
{
    public class Client
    {
        private string fullName;
        private string passportNumber;
        private string phone;
        public string FullName
        {
            get => fullName;

            set
            {
                fullName = value;
            }
        }
        public string PassportNumber
        {
            get => passportNumber;

            set
            {
                passportNumber = value;
            }
        }
        public string Phone
        {
            get => phone;

            set
            {
                phone = value;
            }
        }

        public Client(string fullName, string passportNumber, string phone)
        {
            FullName = fullName;
            PassportNumber = passportNumber;
            Phone = phone;
        }

        public void UpdatePhone(string newPhone)
        {
            Phone = newPhone;
        }
    }

}
