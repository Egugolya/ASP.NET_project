namespace Lesson2.Models.DTO;

/// <summary>Public sender information; excludes password data.</summary>
public class SenderViewDto
{
    /// <summary>The sender's display name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>The sender's email address.</summary>
    public string Email { get; set; } = string.Empty;
}
