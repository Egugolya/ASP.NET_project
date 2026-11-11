namespace Lesson2.Models.DTO;

/// <summary>Authentication token and public sender details.</summary>
public class SenderTokenDto
{
    /// <summary>The token to provide when sending a message.</summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>The token scheme used in the Authorization header.</summary>
    public string TokenType { get; set; } = "Bearer";
    /// <summary>Public details of the authenticated sender.</summary>
    public SenderViewDto Sender { get; set; } = new();
}
