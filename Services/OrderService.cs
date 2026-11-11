using Lesson2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson2.Services;

public class OrderService : IOrderService
{
    private readonly ApiDbContext _dbContext;

    public OrderService(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Order>> GetAllOrdersAsync() => _dbContext.Orders.AsNoTracking().ToListAsync();

    public async Task<Order> AddOrderAsync(Order order)
    {
        order.OrderNumber = 0;
        order.OrderDateTime = DateTimeOffset.UtcNow;
        order.IsCompleted = false;
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateQuantityAsync(int orderNumber, Order update)
    {
        var order = await _dbContext.Orders.FindAsync(orderNumber);
        if (order is null)
            return null;

        order.ProductsQuantity = update.ProductsQuantity;
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateStatusAsync(int orderNumber, Order update)
    {
        var order = await _dbContext.Orders.FindAsync(orderNumber);
        if (order is null)
            return null;

        order.IsCompleted = update.IsCompleted;
        await _dbContext.SaveChangesAsync();
        return order;
    }
}
