using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;
using Lesson2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson2.Controllers;

/// <summary>
/// Manages customer orders.
/// </summary>
[ApiController]
[Route("api/orders")]
[Tags("Orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    /// <summary>Gets all orders.</summary>
    /// <returns>All orders with their textual completion status.</returns>
    [HttpGet]
    [EndpointSummary("Get all orders")]
    [EndpointDescription("Returns every order, including its server-assigned number and completion status.")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(_mapper.Map<List<OrderDto>>(orders));
    }

    /// <summary>Creates an order.</summary>
    /// <param name="request">Client-provided order date, product quantity, cost, and optional comment.</param>
    /// <returns>The created order. The server assigns its number, timestamp, and incomplete status.</returns>
    [HttpPost]
    [EndpointSummary("Create an order")]
    [EndpointDescription("Creates an order. The order number and timestamp are assigned by the server, and a new order is always incomplete.")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> AddOrder([FromBody] AddOrderRequest request)
    {
        var order = _mapper.Map<Order>(request);
        var createdOrder = await _orderService.AddOrderAsync(order);
        var response = _mapper.Map<OrderDto>(createdOrder);
        return Created("/api/orders", response);
    }

    /// <summary>Updates the product quantity for an order.</summary>
    /// <param name="orderNumber">The number of the order to update.</param>
    /// <param name="request">The new positive product quantity.</param>
    /// <returns>The updated order, or 404 when the order does not exist.</returns>
    [HttpPut("{orderNumber:int}/quantity")]
    [EndpointSummary("Update order quantity")]
    [EndpointDescription("Changes only the product quantity. The quantity must be greater than zero.")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> UpdateQuantity(int orderNumber, [FromBody] UpdateOrderQuantityRequest request)
    {
        var update = _mapper.Map<Order>(request);
        var order = await _orderService.UpdateQuantityAsync(orderNumber, update);
        return order is null ? NotFound() : Ok(_mapper.Map<OrderDto>(order));
    }

    /// <summary>Changes whether an order is completed.</summary>
    /// <param name="orderNumber">The number of the order to update.</param>
    /// <param name="request">The new completed or not-completed value.</param>
    /// <returns>The updated order, or 404 when the order does not exist.</returns>
    [HttpPut("{orderNumber:int}/status")]
    [EndpointSummary("Update order status")]
    [EndpointDescription("Changes only the order completion flag and returns its textual status.")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> UpdateStatus(int orderNumber, [FromBody] UpdateOrderStatusRequest request)
    {
        var update = _mapper.Map<Order>(request);
        var order = await _orderService.UpdateStatusAsync(orderNumber, update);
        return order is null ? NotFound() : Ok(_mapper.Map<OrderDto>(order));
    }
}
