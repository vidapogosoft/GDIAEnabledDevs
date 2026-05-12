using MoqDemo.Core.Models;

namespace MoqDemo.Core.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<bool> UpdateStockAsync(int productId, int newStock);
}
