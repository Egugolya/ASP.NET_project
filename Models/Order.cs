using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson2.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderNumber { get; set; }
    public DateOnly OrderDate { get; set; }
    public int ProductsQuantity { get; set; }
    public decimal ProductsCost { get; set; }
    public DateTimeOffset OrderDateTime { get; set; }
    public bool IsCompleted { get; set; }
    public string? Comment { get; set; }
}
