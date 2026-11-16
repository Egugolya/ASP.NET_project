using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Services;

public interface ISenderService
{
    Task<Sender?> RegisterAsync(Sender sender, string password);
    Task<Sender?> AuthenticateAsync(string email, string password);
    Task<ICollection<Sender>> GetAllAsync();
    Task<PaginatedResult<Sender>> GetPaginatedAsync(int page, int pageSize);
    Task<Sender?> GetByEmailAsync(string email);
    Task<bool> DeleteAsync(string email);
    Task<string?> GetEmailForTokenAsync(string token);
}
