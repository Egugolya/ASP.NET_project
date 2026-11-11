using Lesson2.Models;

namespace Lesson2.Services;

public interface IMessageService
{
    Task<List<Message>> GetAllMessagesAsync();
    Task<Message?> GetMessageByIdAsync(int id);
    Task<List<Message>> GetMessagesBySenderAsync(string email);
    Task AddMessageAsync(Message message);
    Task<bool> DeleteMessageAsync(int id);
}
