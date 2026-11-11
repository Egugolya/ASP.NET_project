namespace Lesson2.Models;

public class SenderToken
{
    public string TokenHash { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public Sender Sender { get; set; } = null!;
}
