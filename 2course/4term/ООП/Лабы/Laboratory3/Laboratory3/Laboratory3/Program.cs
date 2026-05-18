using Orders;
using System.Collections.Generic;

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
    DiscountCard = DiscountCardType.Gold 
};

var processor = new OrderProcessor();

try
{
    processor.Process(order);
}
catch (Exception e)
{
    Console.WriteLine($"Failed to process order: {e.Message}");
    throw;
}

Console.WriteLine("\n--- Повторная обработка того же заказа (проверка кэша) ---");
try
{
    processor.Process(order);
}
catch (Exception e)
{
    Console.WriteLine($"Failed to process order: {e.Message}");
    throw;
}

Console.WriteLine("\nTesting Warehouse Stuff:");
var workers = new IWarehouseWorker[]{
        new HumanManager(),
        new RobotPacker() { Model = "George Droid" },
    };

Warehouse.ManageWarehouse(workers);
