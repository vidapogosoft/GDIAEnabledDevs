using MoqDemo.Core.Exceptions;
using MoqDemo.Core.Interfaces;
using MoqDemo.Core.Models;

namespace MoqDemo.Core.Services;

/// <summary>
/// Servicio de pedidos - esta es la clase que vamos a testear con Moq.
/// Depende de varias interfaces que serán mockeadas en los tests.
/// </summary>
public class OrderService
{
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IEmailService _emailService;
    private readonly IAppLogger _logger;

    private const int LowStockThreshold = 5;
    private const string AdminEmail = "admin@tienda.com";

    public OrderService(
        IProductRepository productRepo,
        IOrderRepository orderRepo,
        IEmailService emailService,
        IAppLogger logger)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Crea un pedido validando stock, calculando precio total
    /// y enviando notificaciones.
    /// </summary>
    public async Task<Order> PlaceOrderAsync(int productId, int quantity, string customerEmail)
    {
        _logger.LogInfo($"Iniciando pedido: producto={productId}, cantidad={quantity}");

        // 1. Obtener producto
        var product = await _productRepo.GetByIdAsync(productId)
            ?? throw new DomainException($"Producto {productId} no encontrado.");

        // 2. Validar stock
        if (product.Stock < quantity)
            throw new DomainException(
                $"Stock insuficiente. Disponible: {product.Stock}, solicitado: {quantity}.");

        // 3. Crear pedido
        var order = new Order
        {
            ProductId = productId,
            Quantity = quantity,
            TotalPrice = product.Price * quantity,
            CreatedAt = DateTime.UtcNow,
            Status = "Confirmed"
        };

        var created = await _orderRepo.CreateAsync(order);

        // 4. Actualizar stock
        int newStock = product.Stock - quantity;
        await _productRepo.UpdateStockAsync(productId, newStock);

        // 5. Enviar confirmación al cliente
        await _emailService.SendOrderConfirmationAsync(customerEmail, created.Id);

        // 6. Alerta de stock bajo
        if (newStock < LowStockThreshold)
        {
            _logger.LogWarning($"Stock bajo para producto {productId}: {newStock} unidades.");
            await _emailService.SendLowStockAlertAsync(AdminEmail, productId, newStock);
        }

        _logger.LogInfo($"Pedido {created.Id} creado exitosamente.");
        return created;
    }

    /// <summary>
    /// Obtiene un pedido por ID.
    /// </summary>
    public async Task<Order> GetOrderAsync(int orderId)
    {
        return await _orderRepo.GetByIdAsync(orderId)
            ?? throw new DomainException($"Pedido {orderId} no encontrado.");
    }

    /// <summary>
    /// Elimina un pedido por ID.
    /// </summary>
    public async Task DeleteOrderAsync(int orderId)
    {
        _logger.LogInfo($"Eliminando pedido {orderId}");

        var existing = await _orderRepo.GetByIdAsync(orderId)
            ?? throw new DomainException($"Pedido {orderId} no encontrado.");

        var success = await _orderRepo.DeleteAsync(orderId);

        if (!success)
            throw new InvalidOperationException("No se pudo eliminar el pedido.");

        _logger.LogInfo($"Pedido {orderId} eliminado.");
    }

    /// <summary>
    /// Intenta crear un pedido de forma segura: captura errores del repositorio
    /// y devuelve true si fue creado correctamente, false en caso de fallo.
    /// </summary>
    public async Task<bool> TryCreateOrderAsync(Order order)
    {
        _logger.LogInfo($"Intentando crear pedido para producto {order.ProductId}");

        try
        {
            var created = await _orderRepo.CreateAsync(order);

            if (created == null)
            {
                _logger.LogWarning($"Repositorio devolvió null al crear pedido para producto {order.ProductId}");
                return false;
            }

            _logger.LogInfo($"Pedido {created.Id} creado correctamente.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al crear pedido", ex);
            return false;
        }
    }

    /// <summary>
    /// Actualiza un pedido existente.
    /// </summary>
    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _logger.LogInfo($"Actualizando pedido {order.Id}");

        var existing = await _orderRepo.GetByIdAsync(order.Id)
            ?? throw new DomainException($"Pedido {order.Id} no encontrado.");

        existing.Quantity = order.Quantity;
        existing.TotalPrice = order.TotalPrice;
        existing.Status = order.Status;

        var updated = await _orderRepo.UpdateAsync(existing);

        if (updated == null)
            throw new InvalidOperationException("No se pudo actualizar el pedido.");

        _logger.LogInfo($"Pedido {order.Id} actualizado.");
        return updated;
    }

    /// <summary>
    /// Devuelve el catálogo de productos disponibles.
    /// </summary>
    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        var products = await _productRepo.GetAllAsync();
        return products.Where(p => p.Stock > 0);
    }
}
