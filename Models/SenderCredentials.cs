using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models;

/// <summary>Credentials used to authenticate a sender.</summary>
public class SenderCredentials
{
    /// <summary>The registered sender email address.</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
