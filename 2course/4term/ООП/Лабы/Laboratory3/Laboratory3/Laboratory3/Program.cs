// =========================================================
// Файл: Program.cs
// Описание: Точка входа в приложение.
// =========================================================

using Orders;
using System.Collections.Generic;

// 1. Создание заказа с дисконтной картой
var order = new Order
{
    Id = "ORD-256-X",
    Items = new List<Item> {
        new Item { Id = "1", Name = "Thermal Clips", Price = 1500 },
        new Item { Id = "2", Name = "UNATCO Pass Card", Price = 50 },
    },
    ClientEmail = "jeevacation@gmail.com",
    Type = OrderType.Premium,
    Destination = new Address { City = "Agartha", Street = "33 Thomas Street", Zip = "[REDACTED]" },
    DiscountCard = DiscountCardType.Gold // пример: Gold карта — 15% скидка
};

// 2. Инициализация процессора (по умолчанию создаст кэш, SMTP, Telegram и логгер)
var processor = new OrderProcessor();

// 3. Обработка заказа
try
{
    processor.Process(order);
}
catch (Exception e)
{
    Console.WriteLine($"Failed to process order: {e.Message}");
    throw;
}

// 4. Повторная попытка сохранить тот же заказ — демонстрация кэширования
Console.WriteLine("\n--- Повторная обработка того же заказа (проверка кэша) ---");
try
{
    processor.Process(order); // при сохранении репозиторий должен вывести "Заказ уже есть в кэше" и не писать в файл
}
catch (Exception e)
{
    Console.WriteLine($"Failed to process order: {e.Message}");
    throw;
}

// 5. Работа с обслуживанием (как раньше)
Console.WriteLine("\nTesting Warehouse Stuff:");
var workers = new IWarehouseWorker[]{
        new HumanManager(),
        new RobotPacker() { Model = "George Droid" },
    };

Warehouse.ManageWarehouse(workers);
