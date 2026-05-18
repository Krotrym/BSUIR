namespace Orders;

interface IWarehouseWorker { } 
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
}

static class Warehouse
{
    public static void ManageWarehouse(IWarehouseWorker[] workers)
    {
        Console.WriteLine("\n--- Warehouse Shift Started ---");

        foreach (var worker in workers)
        {
            if (worker is IOrderProcessor orderProcessor)
            {
                orderProcessor.ProcessOrder();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot process orders");
            }

            if (worker is IMeetingParticipant meetingParticipant)
            {
                meetingParticipant.AttendMeeting();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot attend meetings");
            }

            if (worker is IRestable restable)
            {
                restable.GetRest();
            }
            else
            {
                Console.WriteLine("ERROR: Worker cannot rest");
            }

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
