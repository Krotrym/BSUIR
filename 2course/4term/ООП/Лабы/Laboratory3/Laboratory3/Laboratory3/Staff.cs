// =========================================================
// Файл: Staff.cs
// Описание: Система управления персоналом склада.
// =========================================================

namespace Orders;

interface IWarehouseWorker { } // маркерный интерфейс для совместимости с существующим кодом

// Разделяем обязанности на мелкие интерфейсы (Interface Segregation)
interface IOrderProcessor
{
    void ProcessOrder();
}

interface IMeetingParticipant
{
    void AttendMeeting();
}

interface IRestable
{
    void GetRest();
}

interface ITimeWaster
{
    void SwingingTheLead();
}

/// <summary>
/// HumanManager - Человек.
/// </summary>
class HumanManager : IWarehouseWorker, IOrderProcessor, IMeetingParticipant, IRestable, ITimeWaster
{
    public void ProcessOrder()
    {
        Console.WriteLine("Manager is processing logic...");
    }

    public void AttendMeeting()
    {
        Console.WriteLine("Manager is boring at the meeting...");
    }

    public void GetRest()
    {
        Console.WriteLine("Manager is taking a break...");
    }

    public void SwingingTheLead()
    {
        Console.WriteLine("Manager is watching reels...");
    }
}


/// <summary>
/// RobotPacker - Робот.
/// </summary>
class RobotPacker : IWarehouseWorker, IOrderProcessor, IRestable
{
    public required string Model { get; init; }

    public void ProcessOrder()
    {
        Console.WriteLine("Robot " + Model + " is packing boxes...");
    }

    public void GetRest()
    {
        Console.WriteLine("Robot was taken for maintenance");
    }

    // RobotPacker intentionally НЕ реализует IMeetingParticipant и ITimeWaster
    // поведение, которое раньше было в этих методах, теперь обрабатывается в Warehouse.ManageWarehouse
}

static class Warehouse
{
    /// <summary>
    /// ManageWarehouse - функция, которая работает со списком работников.
    /// </summary>
    /// <param name="workers"></param>
    public static void ManageWarehouse(IWarehouseWorker[] workers)
    {
        Console.WriteLine("\n--- Warehouse Shift Started ---");

        foreach (var worker in workers)
        {
            // 1) ProcessOrder — вызываем, если работник умеет обрабатывать заказы
            if (worker is IOrderProcessor orderProcessor)
            {
                orderProcessor.ProcessOrder();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot process orders");
            }

            // 2) AttendMeeting — если работник умеет, вызываем; иначе — сохраняем прежнее поведение для роботов/неучастников
            if (worker is IMeetingParticipant meetingParticipant)
            {
                meetingParticipant.AttendMeeting();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot attend meetings");
            }

            // 3) GetRest — если умеет, вызываем; иначе — логируем
            if (worker is IRestable restable)
            {
                restable.GetRest();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot rest");
            }

            // 4) SwingingTheLead — если умеет, вызываем; иначе — бросаем исключение, как раньше делал робот
            if (worker is ITimeWaster timeWaster)
            {
                timeWaster.SwingingTheLead();
            }
            else
            {
                throw new InvalidOperationException("CRITICAL ERROR: Worker cannot waste our money (we hope so)");
            }
        }
    }
}
