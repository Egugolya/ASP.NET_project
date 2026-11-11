namespace Lesson2.Models.DTO;

/// <summary>Request containing only the replacement completion flag.</summary>
public class UpdateOrderStatusRequest
{
    /// <summary>True when the order is completed; otherwise false.</summary>
    public bool IsCompleted { get; set; }
}
