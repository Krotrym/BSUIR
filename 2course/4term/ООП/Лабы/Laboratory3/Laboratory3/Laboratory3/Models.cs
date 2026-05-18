
namespace Orders;

public enum OrderType
{
    Standard,
    Premium,
    Budget,
    International
}

public enum DiscountCardType
{
    Newbie, 
    Silver, 
    Gold    
}

public record Item
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required double Price { get; init; }
}

public record Address
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string Zip { get; init; }
}
public record Order
{
    public required string Id { get; init; }
    public required IReadOnlyList<Item> Items { get; init; }
    public required OrderType Type { get; init; }
    public required string ClientEmail { get; init; }
    public required Address Destination { get; init; }

    public DiscountCardType DiscountCard { get; init; } = DiscountCardType.Newbie;
}
