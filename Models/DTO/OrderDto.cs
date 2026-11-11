namespace Lesson2.Models.DTO;

/// <summary>Order data returned by the API.</summary>
public class OrderDto
{
    /// <summary>The server-assigned order number.</summary>
    public int OrderNumber { get; set; }
    /// <summary>The calendar date of the order.</summary>
    public DateOnly OrderDate { get; set; }
    /// <summary>The number of products in the order.</summary>
    public int ProductsQuantity { get; set; }
    /// <summary>The total cost of the products.</summary>
    public decimal ProductsCost { get; set; }
    /// <summary>The server-assigned order creation date and time.</summary>
    public DateTimeOffset OrderDateTime { get; set; }
    /// <summary>The completion status as "виконано" or "не виконано".</summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>The optional order comment.</summary>
    public string? Comment { get; set; }
}
