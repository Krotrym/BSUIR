// =========================================================
// Файл: Infrastructure.cs
// Описание: Имитация работы с БД и внешними сервисами.
// =========================================================

namespace Orders;

using System.Collections.Generic;

/// <summary>
/// RandomSQLDatabase - имитация тяжелой базы данных.
/// Реализует IOrderRepository чтобы зависимые компоненты могли работать через абстракцию.
/// </summary>
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

/// <summary>
/// CachedOrderRepository - кэширующая обёртка над IOrderRepository.
/// Если заказ уже в кэше, не обращается к "тяжёлой" БД.
/// </summary>
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

        // Добавляем в кэш до сохранения, чтобы избежать гонок (простая модель)
        _cache.Add(order.Id);
        _inner.SaveOrder(order, total);
    }
}

/// <summary>
/// SmtpMailer - имитация почтового сервиса.
/// Реализует IEmailSender чтобы зависимые компоненты могли работать через абстракцию.
/// </summary>
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

/// <summary>
/// TelegramSender - имитация отправки уведомлений в Telegram.
/// </summary>
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
        // Симуляция отправки в Telegram
        Console.WriteLine($">> Connecting to Telegram Bot {BotToken}...");
        Console.WriteLine($">> Sending TELEGRAM to chat {chatId}\n   Message: {message}");
    }

    // Удобный метод для отправки менеджеру
    public void SendToManager(string message)
    {
        SendMessage(ManagerChatId, message);
    }
}

/// <summary>
/// EventLogger - внутренняя система логирования событий.
/// Записывает события в текстовый лог events_log.txt.
/// </summary>
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
