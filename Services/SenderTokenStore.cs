using System.Collections.Concurrent;

namespace Lesson2.Services;

public class SenderTokenStore
{
    private readonly ConcurrentDictionary<string, string> _tokens = new();

    public string CreateToken(string email)
    {
        var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        _tokens[token] = email;
        return token;
    }

    public string? GetEmailForToken(string token)
    {
        return _tokens.TryGetValue(token, out var email) ? email : null;
    }
}
