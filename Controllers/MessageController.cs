using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;
using Lesson2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Reads, creates, and deletes messages.
/// </summary>
[ApiController]
[Route("messages")]
[Tags("Messages")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _productService;
    private readonly IMapper _mapper;

    public MessageController(IMessageService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <returns>The list of messages.</returns>
    [HttpGet]
    [EndpointSummary("Get all messages")]
    [EndpointDescription("Returns all messages stored by the application.")]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _productService.GetAllMessagesAsync();
        return Ok(_mapper.Map<List<MessageDto>>(messages));
    }

    /// <summary>
    /// Gets a message by its ID.
    /// </summary>
    /// <param name="id">The message ID.</param>
    /// <returns>The requested message.</returns>
    [HttpGet("{id:int}")]
    [EndpointSummary("Get a message by ID")]
    [EndpointDescription("Returns the message with the specified ID, or 404 if it does not exist.")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageById(int id)
    {
        var message = await _productService.GetMessageByIdAsync(id);
        if (message == null) return NotFound();
        return Ok(_mapper.Map<MessageDto>(message));
    }

    /// <summary>
    /// Gets messages from a sender email address.
    /// </summary>
    /// <param name="email">The sender email address.</param>
    /// <returns>Messages sent from the specified email address.</returns>
    [HttpGet("sender")]
    [EndpointSummary("Get messages by sender")]
    [EndpointDescription("Returns messages whose From address matches the specified email.")]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMessagesBySender([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest();

        var messages = await _productService.GetMessagesBySenderAsync(email);
        return Ok(_mapper.Map<List<MessageDto>>(messages));
    }

    /// <summary>
    /// Creates a message after validating the sender token and From address.
    /// </summary>
    /// <param name="request">The message fields.</param>
    /// <returns>The created message.</returns>
    [HttpPost]
    [EndpointSummary("Create a message")]
    [EndpointDescription("Requires a valid sender token. The authenticated email must match the request From address.")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateMessage([FromBody] AddMessageRequest request)
    {
        var newMessage = _mapper.Map<Message>(request);
        await _productService.AddMessageAsync(newMessage);
        return Created("/messages", _mapper.Map<MessageDto>(newMessage));
    }

    /// <summary>
    /// Deletes a message by its ID.
    /// </summary>
    /// <param name="id">The message ID.</param>
    /// <returns>No content when the message was deleted.</returns>
    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a message")]
    [EndpointDescription("Deletes the message with the specified ID.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var success = await _productService.DeleteMessageAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}