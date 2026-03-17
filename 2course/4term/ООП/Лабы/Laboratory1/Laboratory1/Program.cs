namespace Laboratory1
{
    internal class Program
    {
        public static void Main()
        {
            Client client = new Client("Шпаковский Евгений", "HB111111111", "+37529555555");

            BankAccount account = new BankAccount("666", "Rubles");
            account.Deposit(5000);

            ATM atm = new ATM("BimBim", 20000);
            atm.WithdrawCash(500);
            account.Withdraw(500);

            Loan loan = new Loan(10000, 8.5, 24);
            double monthly = loan.CalculateMonthlyPayment();

            CurrencyExchange exchange = new CurrencyExchange("Rubles", "Euro", 0.22);
            double euros = exchange.Convert(1000);

            Investment inv = new Investment("Etf", 2000);
            inv.Increase(150);

            Insurance insurance = new Insurance("life", 300);

            Console.WriteLine($"Баланс после снятия: {account.Balance}");
            Console.WriteLine($"Евро после обмена: {euros}");
            Console.WriteLine($"Платёж по кредиту: {monthly}");
        }
    }
}
