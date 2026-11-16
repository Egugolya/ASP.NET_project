using System.ComponentModel.DataAnnotations;
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
[Route("api/user")]
[Tags("Users")]
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

    /// <summary>Gets all public user details.</summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<SenderViewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SenderViewDto>>> GetAllUsers()
    {
        var users = await _senderService.GetAllAsync();
        return Ok(_mapper.Map<List<SenderViewDto>>(users));
    }

    /// <summary>Gets a user by email address.</summary>
    /// <param name="id">The user's email address.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SenderViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SenderViewDto>> GetUser(string id)
    {
        var user = await _senderService.GetByEmailAsync(id);
        return user is null ? NotFound() : Ok(_mapper.Map<SenderViewDto>(user));
    }

    /// <summary>Gets one page of public user details.</summary>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PaginatedResult<SenderViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResult<SenderViewDto>>> GetPagedUsers(
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        var result = await _senderService.GetPaginatedAsync(page, pageSize);
        return Ok(new PaginatedResult<SenderViewDto>
        {
            Models = _mapper.Map<List<SenderViewDto>>(result.Models),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        });
    }

    /// <summary>Deletes the user associated with the supplied authorization token.</summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteUser([FromHeader(Name = "X-My-authorization")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized();

        token = token.Trim();
        const string bearerPrefix = "Bearer ";
        if (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
            token = token[bearerPrefix.Length..].Trim();

        var email = await _senderService.GetEmailForTokenAsync(token);
        if (email is null)
            return Unauthorized();

        var deleted = await _senderService.DeleteAsync(email);
        return deleted ? NoContent() : Unauthorized();
    }
}
