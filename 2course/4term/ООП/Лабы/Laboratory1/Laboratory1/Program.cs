namespace Laboratory1
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.SetClientData("Иван", "Иванов", "+375295343260");
            Console.WriteLine(client.GetClientInfo());
            Console.WriteLine();

            BankAccount account = new BankAccount();
            account.AccountNumber = "666";
            account.Currency = "USD";
            account.Deposit(50000);
            account.Withdraw(10000);
            account.ShowBalance();
            Console.WriteLine();

            Loan loan = new Loan();
            loan.Amount = 300000;
            loan.InterestRate = 12;
            loan.Months = 12;
            loan.CalculateMonthlyPayment();
            loan.ShowLoanInfo();
            Console.WriteLine();

            Insurance insurance = new Insurance();
            insurance.PolicyNumber = "BimBim";
            insurance.InsuranceType = "Страховка Машины";
            insurance.Activate();
            insurance.ShowInfo();
            Console.WriteLine();

            Transaction transaction = new Transaction();
            transaction.Amount = 1500;
            transaction.Type = "Оплата услуг";
            transaction.Execute();
            Console.WriteLine();

            Card card = new Card();
            card.CardNumber = "1234567887654321";
            Console.WriteLine("Проверка PIN: " + card.CheckPin("1234"));
            card.Block();
            Console.WriteLine();

            Deposit deposit = new Deposit();
            deposit.Amount = 100000;
            deposit.InterestRate = 8;
            deposit.TermMonths = 6;
            deposit.ShowInfo();
            Console.WriteLine();

            BankBranch branch = new BankBranch();
            branch.Address = "ул. Ленина, д.10";
            branch.Phone = "+375295343260";
            Console.WriteLine(branch.GetBranchInfo());
            Console.WriteLine();

            MoneyTransfer transfer = new MoneyTransfer();
            transfer.FromAccount = "666";
            transfer.ToAccount = "777";
            transfer.Amount = 5000;
            transfer.CalculateCommission(1.5);
            transfer.Execute();
            Console.WriteLine();

            Atm atm = new Atm();
            atm.Location = "ТЦ Катапульта";
            atm.IsOnline = true;
            atm.LoadCash(1000000);
            atm.WithdrawCash(10000);
            Console.WriteLine();

            ExchangeRate rate = new ExchangeRate();
            rate.UpdateRate(95.50);
            Console.WriteLine("100 USD = " + rate.Convert(100) + " RUB");
        }
    }
}
