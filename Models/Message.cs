using System.Text.Json.Serialization;

namespace Lesson2.Models;

public class Message
{
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Content { get; set; }
    [JsonPropertyName("DateTime")]
    public DateTimeOffset? DateTime { get; set; }
}
