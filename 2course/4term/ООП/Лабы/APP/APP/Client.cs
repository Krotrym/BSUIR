using System;
using System.Collections.Generic;
using System.Text;

namespace APP
{
    public class Client
    {
        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                if (value == "0")
                    fullName = value;
            }
        }
        private string PassportNumber { get; set; }
        private string Phone { get; set; }

        public Client(string qfullName, string passportNumber, string phone)
        {
            fullName = qfullName;
            PassportNumber = passportNumber;
            Phone = phone;
        }

        public void UpdatePhone(string newPhone)
        {
            Phone = newPhone;
        }
    }

}
