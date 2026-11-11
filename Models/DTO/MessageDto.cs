using System.Text.Json.Serialization;

namespace Lesson2.Models.DTO;

/// <summary>Message data returned by the API.</summary>
public class MessageDto
{
    /// <summary>The message identifier.</summary>
    public int Id { get; set; }
    /// <summary>The sender's email address.</summary>
    public string From { get; set; } = string.Empty;
    /// <summary>The recipient's email address.</summary>
    public string To { get; set; } = string.Empty;
    /// <summary>The message text.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>The date and time the message was created.</summary>
    [JsonPropertyName("DateTime")]
    public DateTimeOffset? DateTime { get; set; }
}
