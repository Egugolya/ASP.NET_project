using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;
using Lesson2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson2.Controllers;

/// <summary>
/// Registers and logins senders.
/// </summary>
[ApiController]
[Route("api/sender")]
[Tags("Sender Authentication")]
public class SenderController : ControllerBase
{
    private readonly ISenderService _senderService;
    private readonly IMapper _mapper;

    public SenderController(ISenderService senderService, IMapper mapper)
    {
        _senderService = senderService;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a new sender and returns an authorization token.
    /// </summary>
    /// <param name="credentials">Sender registration details.</param>
    /// <returns>The sender details and authorization token.</returns>
    [HttpPost("register")]
    [EndpointSummary("Register a new sender")]
    [EndpointDescription("Creates a sender account and returns an authorization token.")]
    [ProducesResponseType(typeof(SenderTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterSenderRequest credentials)
    {
        var sender = _mapper.Map<Sender>(credentials);
        var registeredSender = await _senderService.RegisterAsync(sender, credentials.Password);
        if (registeredSender == null)
            return Conflict(new { message = "An account with this email already exists." });

        return Ok(_mapper.Map<SenderTokenDto>(registeredSender));
    }

    /// <summary>
    /// Authenticates a sender and returns an authorization token.
    /// </summary>
    /// <param name="credentials">Sender login details.</param>
    /// <returns>The sender details and authorization token.</returns>
    [HttpPost("login")]
    [EndpointSummary("Log in a sender")]
    [EndpointDescription("Validates sender credentials and returns an authorization token.")]
    [ProducesResponseType(typeof(SenderTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] SenderCredentials credentials)
    {
        var sender = _mapper.Map<Sender>(credentials);
        var authenticatedSender = await _senderService.AuthenticateAsync(sender.Email, credentials.Password);
        if (authenticatedSender == null)
            return Unauthorized();

        return Ok(_mapper.Map<SenderTokenDto>(authenticatedSender));
    }
}
