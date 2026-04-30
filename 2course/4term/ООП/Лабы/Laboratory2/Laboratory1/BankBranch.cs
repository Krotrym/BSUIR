using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory1
{
    public class BankBranch<TClient> where TClient : Client
    {
        private List<Card> cards = new List<Card>();
        private List<TClient> clients = new List<TClient>();
        private string address;
        private string phone;
        private string workingHours;


        public string Address
        {
            get => address;
            set { address = value; }
        }

        public string Phone
        {
            get => phone;
            set { phone = value; }
        }

        public string WorkingHours
        {
            get => workingHours;
            set { workingHours = value; }
        }

        public BankBranch()
        {
            address = "1";
            phone = "1";
            workingHours = "09:00-18:00";
        }

        public void AddCard(string cardNumber, string pinCode, string cvcCode, string expiryDate, bool isBlocked)
        {
            cards.Add(new Card(cardNumber, pinCode, cvcCode, expiryDate, isBlocked));
        }
        public void AddClients(TClient client)
        {
                
            if(clients.Any(g => g.ID == client.ID))
            {
                return;
            }
            clients.Add(client);
        }
        public TClient FindClient(string id)
        {
            return clients.Find(c => c.ID == id);
        }

        public BankAccount OpenDebitAccount(Client client, string email, string password, string currency, string id)
        {
            BankDebitAccount account = new BankDebitAccount(email, password, currency, id);
            client.AddAccount(account);
            return account;
        }

        public BankAccount OpenCreditAccount(Client client, string email, string password, string currency, double ammountCredit, string id)
        {
            BankCreditAccount account = new BankCreditAccount(email, password, currency, ammountCredit, id);
            client.AddAccount(account);
            return account;
        }

        public void Deposit(Client client, string id, double amount)
        {
            try
            {
                BankAccount account = client.GetAccount(id);
                account.Deposit(amount);

                Console.WriteLine($"Операция выполнена: пополнение счета {id}");
                Console.WriteLine($"Сумма: {amount} {account.Currency}");
                Console.WriteLine($"Новый баланс: {account.Balance} {account.Currency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }

        public void Withdraw(Client client, string id, double amount)
        {
            try
            {
                BankAccount account = client.GetAccount(id);
                account.Withdraw(amount);

                Console.WriteLine($"Операция выполнена: понижение счёта {id}");
                Console.WriteLine($"Сумма: {amount} {account.Currency}");
                Console.WriteLine($"Новый баланс: {account.Balance} {account.Currency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }
        public void PayClientCredit(Client client, string id, double amount)
        {
            BankAccount account = client.GetAccount(id);
            if (account is not BankCreditAccount creditAccount)
                throw new InvalidOperationException("Операция доступна только для кредитных счетов");

            creditAccount.PayCredit(amount);
        }

        public void PayClientCredit(Client client, string id, int amount)
        {
            BankAccount account = client.GetAccount(id);
            if (account is not BankCreditAccount creditAccount)
                throw new InvalidOperationException("Операция доступна только для кредитных счетов");

            creditAccount.PayCredit(amount);
        }
        public string GetBranchInfo()
        {
            return "Отделение адрес: " + address + ", тел: " + phone;
        }
    }

}
