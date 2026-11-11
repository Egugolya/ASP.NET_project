using System.Security.Cryptography;
using System.Text;
using Lesson2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Services;

public class SenderService : ISenderService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 210_000;

    private readonly IRepository<Sender> _senderRepository;
    private readonly ApiDbContext _dbContext;

    public SenderService(IRepository<Sender> senderRepository, ApiDbContext dbContext)
    {
        _senderRepository = senderRepository;
        _dbContext = dbContext;
    }

    public async Task<Sender?> RegisterAsync(Sender sender, string password)
    {
        var normalizedEmail = sender.Email.Trim().ToLowerInvariant();
        if (await _senderRepository.GetByIdAsync(normalizedEmail) != null)
            return null;

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        sender.Name = sender.Name.Trim();
        sender.Email = normalizedEmail;
        sender.PasswordSalt = Convert.ToBase64String(salt);
        sender.PasswordHash = Convert.ToBase64String(hash);
        _senderRepository.Add(sender);
        await _senderRepository.SaveChangesAsync();

        sender.Token = await CreateTokenAsync(normalizedEmail);
        return sender;
    }

    public async Task<Sender?> AuthenticateAsync(string email, string password)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var sender = await _senderRepository.GetByIdAsync(normalizedEmail);
        if (sender == null)
            return null;

        var salt = Convert.FromBase64String(sender.PasswordSalt);
        var expectedHash = Convert.FromBase64String(sender.PasswordHash);
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        if (!CryptographicOperations.FixedTimeEquals(actualHash, expectedHash))
            return null;

        sender.Token = await CreateTokenAsync(normalizedEmail);
        return sender;
    }

    public async Task<string?> GetEmailForTokenAsync(string token)
    {
        var tokenHash = HashToken(token);
        var storedToken = await _dbContext.SenderTokens
            .AsNoTracking()
            .SingleOrDefaultAsync(storedToken => storedToken.TokenHash == tokenHash);

        return storedToken?.SenderEmail;
    }

    private async Task<string> CreateTokenAsync(string email)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        _dbContext.SenderTokens.Add(new SenderToken
        {
            TokenHash = HashToken(token),
            SenderEmail = email
        });
        await _dbContext.SaveChangesAsync();

        return token;
    }

    private static string HashToken(string token)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}
