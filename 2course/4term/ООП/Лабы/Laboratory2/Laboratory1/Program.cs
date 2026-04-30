namespace Laboratory1
{
    class Program
    {
        static void Main(string[] args)
        {
            BankBranch<Client> branch = new BankBranch<Client>();

            Client client1 = new Client("Pasha", "1", "1", "1");
            Client client2 = new Client("Maksim", "2", "2", "2");

            branch.AddClients(client1);
            branch.AddClients(client2);

            BankAccount creditAccount = branch.OpenCreditAccount(client1, "pasha@gmail.com", "1234", "Rub", 1000, "ID1");
            BankAccount debitAccount = branch.OpenDebitAccount(client2, "maksim@gmail.com", "1111", "USD", "ID2");
            

            branch.Deposit(client1, "ID1", 100);
            branch.Deposit(client2 , "ID2", 100);

            client1.ShowInfo();
            client2.ShowInfo();

            branch.Withdraw(client1, "ID1", 10);
            branch.Withdraw(client2, "ID2", 10);

            client1.ShowInfo();
            client2.ShowInfo();

            branch.PayClientCredit(client1, "ID1", 5.0);
            //branch.PayClientCredit(client2, "ID2", 2);
            client1.ShowInfo();

        }
    }
}
