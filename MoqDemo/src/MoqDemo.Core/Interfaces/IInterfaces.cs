using MoqDemo.Core.Models;

namespace MoqDemo.Core.Interfaces;

// ─── Repositorio de Productos ───────────────────────────────────────────────
/*
public interface IProductRepository
{
    Product? GetById(int id);
    IEnumerable<Product> GetAll();
    void Update(Product product);
}

// ─── Repositorio de Órdenes ─────────────────────────────────────────────────
public interface IOrderRepository
{
    Order Save(Order order);
    Order? GetById(int id);
}
*/

// ─── Servicio de Notificaciones ──────────────────────────────────────────────
public interface INotificationService
{
    void SendOrderConfirmation(Order order);
    void SendLowStockAlert(Product product);
}

// ─── Servicio de Descuentos ──────────────────────────────────────────────────
public interface IDiscountService
{
    decimal GetDiscountPercentage(int productId, int quantity);
}
