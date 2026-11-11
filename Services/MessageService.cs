using Lesson2.Database;
using Lesson2.Models;

namespace Lesson2.Services;

public class MessageService : IMessageService
{
    private readonly IMessageProvider _messageProvider;

    public MessageService(IMessageProvider messageProvider)
    {
        _messageProvider = messageProvider;
    }

    public async Task<List<Message>> GetAllMessagesAsync()
    {
        return await _messageProvider.LoadAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        var messages = await GetAllMessagesAsync();
        return messages.FirstOrDefault(message => message.Id == id);
    }

    public async Task<List<Message>> GetMessagesBySenderAsync(string email)
    {
        var messages = await GetAllMessagesAsync();
        return messages
            .Where(message => string.Equals(message.From, email, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task AddMessageAsync(Message message)
    {
        var messages = await GetAllMessagesAsync();
        message.Id = messages.Count == 0 ? 0 : messages.Max(m => m.Id) + 1;
        message.DateTime = DateTimeOffset.Now;
        messages.Add(message);
        await _messageProvider.SaveAsync(messages);
    }

    public async Task<bool> DeleteMessageAsync(int id)
    {
        var messages = await GetAllMessagesAsync();
        var message = messages.FirstOrDefault(candidate => candidate.Id == id);
        if (message == null)
            return false;

        messages.Remove(message);
        await _messageProvider.SaveAsync(messages);
        return true;
    }
}
