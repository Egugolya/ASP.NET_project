using System.Text.Json.Serialization;

namespace Lesson2.Models;

public class Sender
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;

    [JsonIgnore]
    public string? Token { get; set; }
}
