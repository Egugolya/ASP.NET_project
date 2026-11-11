using Lesson2.Models;

namespace Lesson2.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> AddOrderAsync(Order order);
    Task<Order?> UpdateQuantityAsync(int orderNumber, Order update);
    Task<Order?> UpdateStatusAsync(int orderNumber, Order update);
}
