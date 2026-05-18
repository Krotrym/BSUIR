namespace Orders;

using System.Collections.Generic;
class RandomSQLDatabase : IOrderRepository
{
    public string ConnectionString { get; set; } = "random://root:password@localhost:228/shop";

    public RandomSQLDatabase() { }

    public RandomSQLDatabase(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void SaveOrder(Order order, double total)
    {
        Console.WriteLine($"Connecting to RandomSQL at {ConnectionString} ...");
        Thread.Sleep(500);

        using var file = new StreamWriter("orders_db.txt", new FileStreamOptions()
        {
            Access = FileAccess.Write,
            Mode = FileMode.Append
        });
        var @record = $"[{DateTime.Now:yyyy-MM-ddTHH:mm:ssK}] ID: {order.Id} | Type: {order.Type} | Total: {total:F2}\n";
        file.Write(@record);

        Console.WriteLine("Order saved successfully.");
    }
}
class CachedOrderRepository : IOrderRepository
{
    private readonly IOrderRepository _inner;
    private readonly HashSet<string> _cache = new();

    public CachedOrderRepository(IOrderRepository inner)
    {
        _inner = inner;
    }

    public void SaveOrder(Order order, double total)
    {
        if (_cache.Contains(order.Id))
        {
            Console.WriteLine("Заказ уже есть в кэше");
            return;
        }

        _cache.Add(order.Id);
        _inner.SaveOrder(order, total);
    }
}

class SmtpMailer : IEmailSender
{
    public required string Server { get; set; }

    public SmtpMailer() { }

    public SmtpMailer(string server)
    {
        Server = server;
    }

    public void SendHtmlEmail(string to, string subject, string body)
    {
        Console.WriteLine($">> Connecting to SMTP server {Server}...");
        Console.WriteLine($">> Sending EMAIL to {to}\n   Subject: {subject}\n   Body: {body}");
    }
}

class TelegramSender : ITelegramSender
{
    public required string BotToken { get; init; }
    public required string ManagerChatId { get; init; }

    public TelegramSender() { }

    public TelegramSender(string botToken, string managerChatId)
    {
        BotToken = botToken;
        ManagerChatId = managerChatId;
    }

    public void SendMessage(string chatId, string message)
    {
        Console.WriteLine($">> Connecting to Telegram Bot {BotToken}...");
        Console.WriteLine($">> Sending TELEGRAM to chat {chatId}\n   Message: {message}");
    }

    public void SendToManager(string message)
    {
        SendMessage(ManagerChatId, message);
    }
}
class EventLogger : IEventLogger
{
    private readonly string _filePath;

    public EventLogger(string filePath = "events_log.txt")
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        var @record = $"[{DateTime.Now:yyyy-MM-ddTHH:mm:ssK}] {message}\n";
        using var file = new StreamWriter(_filePath, new FileStreamOptions()
        {
            Access = FileAccess.Write,
            Mode = FileMode.Append
        });
        file.Write(@record);

        Console.WriteLine($">> Event logged: {message}");
    }
}
