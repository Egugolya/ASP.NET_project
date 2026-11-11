using Lesson2.Models;

namespace Lesson2.Database;

public interface IMessageProvider
{
    Task<List<Message>> LoadAsync();
    Task SaveAsync(List<Message> data);
}
