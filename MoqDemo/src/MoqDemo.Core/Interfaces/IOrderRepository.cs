using MoqDemo.Core.Models;

namespace MoqDemo.Core.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<Order> UpdateAsync(Order order);
}
