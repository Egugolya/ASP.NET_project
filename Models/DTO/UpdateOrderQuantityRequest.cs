using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO;

/// <summary>Request containing only the replacement product quantity.</summary>
public class UpdateOrderQuantityRequest
{
    /// <summary>The new product count; must be greater than zero.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "The product quantity must be greater than zero.")]
    public int ProductsQuantity { get; set; }
}
