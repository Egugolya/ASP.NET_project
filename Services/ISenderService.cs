using Lesson2.Models;

namespace Lesson2.Services;

public interface ISenderService
{
    Task<Sender?> RegisterAsync(Sender sender, string password);
    Task<Sender?> AuthenticateAsync(string email, string password);
    string? GetEmailForToken(string token);
}
