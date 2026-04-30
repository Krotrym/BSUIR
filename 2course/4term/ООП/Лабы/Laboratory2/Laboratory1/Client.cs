using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Laboratory1
{
    public class Client
    {
        private string id;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private Insurance insurance;
        private List<BankAccount> bankAccounts = new List<BankAccount>();

        public Client(string firstName, string lastName, string phoneNumber, string iD)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            ID = iD;
        }

        public string ID
        {
            get => id; set => id = value;
        }
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
        public void AddAccount(BankAccount account)
        {
            bankAccounts.Add(account);
        }

        public BankAccount GetAccount(string accountId)
        {
            return bankAccounts.Find(g => g.idAccount == accountId);
        }

        public void ShowInfo()
        {
            for (int i = 0; i < bankAccounts.Count; i++)
            {
                bankAccounts[i].ShowInfo();
            }
        }
    }

}
