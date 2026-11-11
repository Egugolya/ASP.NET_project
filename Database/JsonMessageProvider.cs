using Lesson2.Models;
using System.Text.Json;

namespace Lesson2.Database;

public class JsonMessageProvider : IMessageProvider
{
    private readonly string _filePath;

    public JsonMessageProvider(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<Message>> LoadAsync()
    {
        if (!File.Exists(_filePath)) return new List<Message>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Message>>(json) ?? new List<Message>();
    }

    public async Task SaveAsync(List<Message> data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}