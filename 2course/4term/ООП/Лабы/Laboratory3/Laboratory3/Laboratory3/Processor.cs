namespace Orders;

using System;
using System.Collections.Generic;

interface IOrderRepository
{
    void SaveOrder(Order order, double total);
}

interface IEmailSender
{
    void SendHtmlEmail(string to, string subject, string body);
}

interface ITelegramSender
{
    void SendMessage(string chatId, string message);
    void SendToManager(string message);
}

interface IEventLogger
{
    void Log(string message);
}

interface IOrderValidator
{
    void Validate(Order order);
}

interface IPriceCalculator
{
    double CalculateTotal(Order order);
}

class DefaultOrderValidator : IOrderValidator
{
    public void Validate(Order order)
    {
        if (order.Items == null || order.Items.Count == 0)
            throw new ArgumentException("order must have at least one item", nameof(order));

        if (order.Destination == null || order.Destination.City == "")
            throw new ArgumentException("destination city is required", nameof(order));
    }
}
class DefaultPriceCalculator : IPriceCalculator
{
    public double CalculateTotal(Order order)
    {
        double total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price;
        }

        switch (order.Type)
        {
            case OrderType.Standard:
                total = total * 1.2;
                break;
            case OrderType.Premium:
                total = (total * 0.9) * 1.2;
                break;
            case OrderType.Budget:
                if (order.Items.Count > 3)
                {
                    Console.WriteLine("Budget orders cannot have more than 3 items. Skipping.");
                    return double.NaN;
                }
                break;
            case OrderType.International:
                total = total * 1.5;
                if (order.Destination.City == "Nowhere")
                {
                    throw new ArgumentException("cannot ship to Nowhere", nameof(order));
                }
                break;
            default:
                throw new ArgumentException("unknown order type", nameof(order));
        }

        var discountPercent = GetDiscountPercent(order.DiscountCard);
        if (discountPercent > 0)
        {
            var discountAmount = total * discountPercent;
            total -= discountAmount;
            Console.WriteLine($"Applied discount {discountPercent * 100:F0}% (amount {discountAmount:F2}) for card {order.DiscountCard}");
        }

        return total;
    }

    private double GetDiscountPercent(DiscountCardType card)
    {
        return card switch
        {
            DiscountCardType.Gold => 0.15,
            DiscountCardType.Silver => 0.10,
            _ => 0.0
        };
    }
}

class OrderProcessor
{
    private readonly IOrderRepository _repository;
    private readonly IEmailSender _emailSender;
    private readonly ITelegramSender _telegramSender;
    private readonly IEventLogger _eventLogger;
    private readonly IOrderValidator _validator;
    private readonly IPriceCalculator _calculator;

    public OrderProcessor()
        : this(
            new CachedOrderRepository(new RandomSQLDatabase()),
            new SmtpMailer() { Server = "smtp.google.com" },
            new TelegramSender() { BotToken = "BOT-TOKEN-PLACEHOLDER", ManagerChatId = "MANAGER-CHAT-ID" },
            new EventLogger(),
            new DefaultOrderValidator(),
            new DefaultPriceCalculator())
    { }

    public OrderProcessor(
        IOrderRepository repository,
        IEmailSender emailSender,
        ITelegramSender telegramSender,
        IEventLogger eventLogger,
        IOrderValidator validator,
        IPriceCalculator calculator)
    {
        _repository = repository;
        _emailSender = emailSender;
        _telegramSender = telegramSender;
        _eventLogger = eventLogger;
        _validator = validator;
        _calculator = calculator;
    }

    public void Process(Order order)
    {
        Console.WriteLine($"--- Processing Order {order.Id} ---");

        _validator.Validate(order);
        double total = _calculator.CalculateTotal(order);

        if (double.IsNaN(total))
            return;

        try
        {
            _repository.SaveOrder(order, total);
        }
        catch (Exception e)
        {
            Console.WriteLine($"database error: {e.Message}");
            throw;
        }

        var emailBody = $"<h1>Your order {order.Id} is confirmed!</h1><p>Total: {total:F2}</p>";
        _emailSender.SendHtmlEmail(order.ClientEmail, "Order Confirmation", emailBody);

        var telegramMessage = $"Order {order.Id} confirmed. Total: {total:F2}. Client: {order.ClientEmail}";
        _telegramSender.SendToManager(telegramMessage);

        var logMessage = $"Order {order.Id} processed. Total: {total:F2}. DiscountCard: {order.DiscountCard}";
        _eventLogger.Log(logMessage);
    }
}
