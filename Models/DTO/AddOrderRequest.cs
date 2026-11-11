using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO;

/// <summary>Client-provided fields used to create an order.</summary>
public class AddOrderRequest
{
    /// <summary>The calendar date of the order.</summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>The number of products in the order; must be greater than zero.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "The product quantity must be greater than zero.")]
    public int ProductsQuantity { get; set; }

    /// <summary>The total cost of the products; must be greater than zero.</summary>
    [PositiveDecimal]
    public decimal ProductsCost { get; set; }

    /// <summary>An optional comment of up to 200 characters.</summary>
    [StringLength(200, ErrorMessage = "The comment must be no longer than 200 characters.")]
    public string? Comment { get; set; }
}
