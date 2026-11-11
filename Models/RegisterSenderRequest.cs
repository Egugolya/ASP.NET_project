using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models;

/// <summary>Fields required to register a sender account.</summary>
public class RegisterSenderRequest
{
    /// <summary>The sender's display name.</summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
