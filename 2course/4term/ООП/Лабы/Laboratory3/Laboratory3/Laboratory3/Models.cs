// =========================================================
// Файл: Models.cs
// Описание: Модели данных системы.
// =========================================================

namespace Orders;

public enum OrderType
{
    Standard,
    Premium,
    Budget,
    International
}

/// <summary>
/// DiscountCardType - типы дисконтных карт.
/// </summary>
public enum DiscountCardType
{
    Newbie, // 0%
    Silver, // 10%
    Gold    // 15%
}

/// <summary>
/// Item - товар в заказе.
/// immutable record для однозначного представления данных.
/// </summary>
public record Item
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required double Price { get; init; }
}

/// <summary>
/// Address - адрес доставки.
/// immutable record для простоты тестирования и безопасности данных.
/// </summary>
public record Address
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string Zip { get; init; }
}

/// <summary>
/// Order - заказ.
/// Добавлено поле DiscountCard для указания типа дисконтной карты клиента.
/// </summary>
public record Order
{
    public required string Id { get; init; }
    public required IReadOnlyList<Item> Items { get; init; }
    public required OrderType Type { get; init; }
    public required string ClientEmail { get; init; }
    public required Address Destination { get; init; }

    // Новое поле: тип дисконтной карты клиента (по умолчанию Newbie)
    public DiscountCardType DiscountCard { get; init; } = DiscountCardType.Newbie;
}
