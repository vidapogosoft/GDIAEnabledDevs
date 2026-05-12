using FluentAssertions;
using Moq;
using MoqDemo.Core.Exceptions;
using MoqDemo.Core.Interfaces;
using MoqDemo.Core.Models;
using MoqDemo.Core.Services;
using Xunit;

namespace MoqDemo.Tests;

/// <summary>
/// ============================================================
///  DEMO DE MOQ EN C# .NET 8
///  Moq es el equivalente de Mockito en el ecosistema .NET.
/// 
///  Conceptos cubiertos:
///  1. Crear mocks básicos             (Mock<T>)
///  2. Configurar retornos             (Setup + Returns / ReturnsAsync)
///  3. Verificar llamadas              (Verify)
///  4. Verificar que NO se llamó algo  (Verify con Times.Never)
///  5. Lanzar excepciones desde mocks  (Throws / ThrowsAsync)
///  6. Capturar argumentos             (Callback)
///  7. Retornos condicionales          (It.Is<T>)
///  8. Propiedades mockeadas           (SetupProperty / SetupGet)
///  9. MockBehavior.Strict             (falla si se llama algo no configurado)
/// ============================================================
/// </summary>
public class OrderServiceTests
{
    // ── Dependencias mockeadas (equivalente a @Mock en Mockito) ──
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IAppLogger> _loggerMock;

    // ── Sistema bajo prueba (equivalente a @InjectMocks en Mockito) ──
    private readonly OrderService _sut;

    // Datos de prueba reutilizables
    private readonly Product _sampleProduct = new()
    {
        Id = 1,
        Name = "Laptop Pro",
        Price = 1500m,
        Stock = 10
    };

    public OrderServiceTests()
    {
        // 1. CREAR MOCKS
        // En Mockito: @Mock / Mockito.mock(IProductRepository.class)
        // En Moq:     new Mock<IProductRepository>()
        _productRepoMock = new Mock<IProductRepository>();
        _orderRepoMock   = new Mock<IOrderRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock      = new Mock<IAppLogger>();

        // 2. INYECTAR MOCKS (creamos el SUT con los mocks como dependencias)
        // En Mockito esto lo hace @InjectMocks automáticamente
        _sut = new OrderService(
            _productRepoMock.Object,   // .Object = la instancia mockeada
            _orderRepoMock.Object,
            _emailServiceMock.Object,
            _loggerMock.Object);
    }

    // ================================================================
    // GRUPO 1: CONFIGURAR RETORNOS (Setup + ReturnsAsync)
    // ================================================================

    [Fact(DisplayName = "1a. PlaceOrder - crea pedido con datos correctos")]
    public async Task PlaceOrder_WhenProductExistsAndHasStock_ReturnsCreatedOrder()
    {
        // ARRANGE
        // En Mockito: when(productRepo.getById(1)).thenReturn(product)
        // En Moq:     Setup(...).ReturnsAsync(...)
        _productRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(_sampleProduct);

        var expectedOrder = new Order { Id = 99, ProductId = 1, Quantity = 2, TotalPrice = 3000m };
        _orderRepoMock
            .Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync(expectedOrder);

        _productRepoMock
            .Setup(r => r.UpdateStockAsync(1, It.IsAny<int>()))
            .ReturnsAsync(true);

        _emailServiceMock
            .Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.PlaceOrderAsync(1, 2, "cliente@email.com");

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(99);
        result.TotalPrice.Should().Be(3000m);
    }

    // ================================================================
    // GRUPO 2: VERIFICAR QUE SE LLAMARON MÉTODOS (Verify)
    // ================================================================

    [Fact(DisplayName = "2a. PlaceOrder - verifica que se envió email de confirmación")]
    public async Task PlaceOrder_ShouldSendConfirmationEmail_ToCustomer()
    {
        // ARRANGE
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct);
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 1 });
        _productRepoMock.Setup(r => r.UpdateStockAsync(1, It.IsAny<int>())).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT
        await _sut.PlaceOrderAsync(1, 2, "cliente@email.com");

        // ASSERT - Verificar interacciones
        // En Mockito: verify(emailService).sendOrderConfirmation("cliente@email.com", 1)
        // En Moq:     Verify(e => e.SendOrderConfirmationAsync(...), Times.Once())
        _emailServiceMock.Verify(
            e => e.SendOrderConfirmationAsync("cliente@email.com", 1),
            Times.Once(),
            "Debió enviarse exactamente 1 email de confirmación al cliente");
    }

    [Fact(DisplayName = "2b. PlaceOrder - verifica que se actualizó el stock")]
    public async Task PlaceOrder_ShouldUpdateStock_AfterCreatingOrder()
    {
        // ARRANGE
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct);
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 5 });
        _productRepoMock.Setup(r => r.UpdateStockAsync(1, 8)).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT
        await _sut.PlaceOrderAsync(1, 2, "x@x.com");  // Stock era 10, compramos 2 → queda 8

        // ASSERT
        _productRepoMock.Verify(
            r => r.UpdateStockAsync(1, 8),  // Verificamos el valor exacto del nuevo stock
            Times.Once());
    }

    // ================================================================
    // GRUPO 3: VERIFICAR QUE NO SE LLAMÓ UN MÉTODO (Times.Never)
    // ================================================================

    [Fact(DisplayName = "3a. PlaceOrder - NO envía alerta de stock si queda suficiente")]
    public async Task PlaceOrder_WhenStockRemainsAboveThreshold_ShouldNotSendLowStockAlert()
    {
        // ARRANGE - compramos 1 unidad, quedan 9 (> threshold de 5)
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct);
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 1 });
        _productRepoMock.Setup(r => r.UpdateStockAsync(1, 9)).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT
        await _sut.PlaceOrderAsync(1, 1, "x@x.com");

        // ASSERT - Times.Never() es equivalente a Mockito verify(x, never())
        _emailServiceMock.Verify(
            e => e.SendLowStockAlertAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
            Times.Never(),
            "No debería alertar cuando el stock es suficiente");
    }

    [Fact(DisplayName = "3b. PlaceOrder - SÍ envía alerta de stock bajo cuando corresponde")]
    public async Task PlaceOrder_WhenStockDropsBelowThreshold_ShouldSendLowStockAlert()
    {
        // ARRANGE - producto con stock = 5, compramos 3, quedan 2 (< threshold de 5)
        var lowStockProduct = new Product { Id = 2, Name = "Mouse", Price = 30m, Stock = 5 };

        _productRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(lowStockProduct);
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 10 });
        _productRepoMock.Setup(r => r.UpdateStockAsync(2, 2)).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);
        _emailServiceMock.Setup(e => e.SendLowStockAlertAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT
        await _sut.PlaceOrderAsync(2, 3, "x@x.com");

        // ASSERT
        _emailServiceMock.Verify(
            e => e.SendLowStockAlertAsync("admin@tienda.com", 2, 2),
            Times.Once());
    }

    // ================================================================
    // GRUPO 4: LANZAR EXCEPCIONES DESDE MOCKS (ThrowsAsync)
    // ================================================================

    [Fact(DisplayName = "4a. PlaceOrder - lanza excepción si producto no existe")]
    public async Task PlaceOrder_WhenProductNotFound_ThrowsDomainException()
    {
        // ARRANGE
        // En Mockito: when(repo.getById(99)).thenReturn(null)
        // En Moq:     Setup(...).ReturnsAsync((Product?)null)
        _productRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // ACT & ASSERT
        var act = () => _sut.PlaceOrderAsync(99, 1, "x@x.com");

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*99*no encontrado*");
    }

    [Fact(DisplayName = "4b. PlaceOrder - lanza excepción si stock insuficiente")]
    public async Task PlaceOrder_WhenInsufficientStock_ThrowsDomainException()
    {
        // ARRANGE - intentamos comprar más de lo disponible
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct); // Stock = 10

        // ACT & ASSERT
        var act = () => _sut.PlaceOrderAsync(1, 50, "x@x.com"); // Pedimos 50

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*Stock insuficiente*");
    }

    [Fact(DisplayName = "4c. PlaceOrder - propaga excepción si el repositorio falla")]
    public async Task PlaceOrder_WhenRepositoryThrows_ExceptionPropagates()
    {
        // ARRANGE
        // En Mockito: when(repo.getById(1)).thenThrow(new RuntimeException("DB error"))
        // En Moq:     Setup(...).ThrowsAsync(new Exception("DB error"))
        _productRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ThrowsAsync(new InvalidOperationException("Error de base de datos"));

        // ACT & ASSERT
        var act = () => _sut.PlaceOrderAsync(1, 1, "x@x.com");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Error de base de datos");
    }

    // ================================================================
    // GRUPO 5: CAPTURAR ARGUMENTOS (Callback)
    // ================================================================

    [Fact(DisplayName = "5a. Callback - captura el objeto Order que se guardó")]
    public async Task PlaceOrder_ShouldCreateOrderWithCorrectValues()
    {
        // ARRANGE
        Order? capturedOrder = null; // Aquí guardaremos el argumento capturado

        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct);

        // Callback captura el argumento - equivalente a ArgumentCaptor en Mockito
        _orderRepoMock
            .Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .Callback<Order>(order => capturedOrder = order)  // ← Aquí capturamos
            .ReturnsAsync(new Order { Id = 1 });

        _productRepoMock.Setup(r => r.UpdateStockAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT
        await _sut.PlaceOrderAsync(1, 3, "x@x.com");

        // ASSERT - validamos el objeto capturado
        capturedOrder.Should().NotBeNull();
        capturedOrder!.ProductId.Should().Be(1);
        capturedOrder.Quantity.Should().Be(3);
        capturedOrder.TotalPrice.Should().Be(4500m);   // 1500 * 3
        capturedOrder.Status.Should().Be("Confirmed");
    }

    // ================================================================
    // GRUPO 6: It.Is<T> - Retornos condicionales por argumento
    // ================================================================

    [Fact(DisplayName = "6a. It.Is<T> - configura respuestas distintas según argumento")]
    public async Task GetAvailableProducts_ReturnsOnlyProductsWithStock()
    {
        // ARRANGE
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop",  Price = 1000m, Stock = 5  },
            new() { Id = 2, Name = "Teclado", Price = 50m,   Stock = 0  }, // Sin stock
            new() { Id = 3, Name = "Mouse",   Price = 30m,   Stock = 3  },
        };

        _productRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);

        // ACT
        var available = await _sut.GetAvailableProductsAsync();

        // ASSERT - solo deben aparecer los que tienen stock > 0
        available.Should().HaveCount(2);
        available.Should().NotContain(p => p.Name == "Teclado");
    }

    [Fact(DisplayName = "6b. It.Is<T> - setup diferente para diferentes IDs")]
    public async Task GetOrder_WithDifferentIds_ReturnsDifferentOrders()
    {
        // ARRANGE
        // En Mockito: when(repo.getById(1)).thenReturn(order1)
        //             when(repo.getById(2)).thenReturn(order2)
        _orderRepoMock
            .Setup(r => r.GetByIdAsync(It.Is<int>(id => id == 1)))
            .ReturnsAsync(new Order { Id = 1, Status = "Confirmed" });

        _orderRepoMock
            .Setup(r => r.GetByIdAsync(It.Is<int>(id => id == 2)))
            .ReturnsAsync(new Order { Id = 2, Status = "Shipped" });

        // ACT
        var order1 = await _sut.GetOrderAsync(1);
        var order2 = await _sut.GetOrderAsync(2);

        // ASSERT
        order1.Status.Should().Be("Confirmed");
        order2.Status.Should().Be("Shipped");
    }

    // ================================================================
    // GRUPO 7: VERIFICAR NÚMERO EXACTO DE LLAMADAS
    // ================================================================

    [Fact(DisplayName = "7a. Verify Times - verifica cantidad exacta de llamadas al logger")]
    public async Task PlaceOrder_ShouldLogTwice_OnSuccess()
    {
        // ARRANGE
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(_sampleProduct);
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 1 });
        _productRepoMock.Setup(r => r.UpdateStockAsync(1, It.IsAny<int>())).ReturnsAsync(true);
        _emailServiceMock.Setup(e => e.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
                         .Returns(Task.CompletedTask);

        // ACT - compramos 1, quedan 9, sin alerta de stock
        await _sut.PlaceOrderAsync(1, 1, "x@x.com");

        // ASSERT
        // Deben haberse llamado exactamente 2 LogInfo (inicio + fin del pedido)
        _loggerMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Exactly(2));

        // Y ningún LogWarning (stock suficiente)
        _loggerMock.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Never());
    }

    // ================================================================
    // GRUPO 8: VerifyNoOtherInteractions (VerifyNoOtherCalls en Moq)
    // ================================================================

    [Fact(DisplayName = "8a. GetOrder - solo interactúa con IOrderRepository")]
    public async Task GetOrder_ShouldOnlyInteractWithOrderRepository()
    {
        // ARRANGE
        _orderRepoMock
            .Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync(new Order { Id = 42 });

        // ACT
        await _sut.GetOrderAsync(42);

        // ASSERT - verificamos que NO se tocaron otros repositorios
        // En Mockito: verifyNoInteractions(productRepo, emailService, logger)
        _productRepoMock.VerifyNoOtherCalls();
        _emailServiceMock.VerifyNoOtherCalls();
    }

    [Fact(DisplayName = "9a. DeleteOrder - elimina pedido existente correctamente")]
    public async Task DeleteOrder_WhenOrderExists_DeletesAndLogs()
    {
        // ARRANGE
        var existing = new Order { Id = 7, ProductId = 1, Quantity = 1 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
        _orderRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);

        // ACT
        await _sut.DeleteOrderAsync(7);

        // ASSERT
        _orderRepoMock.Verify(r => r.DeleteAsync(7), Times.Once());
        _loggerMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact(DisplayName = "9b. DeleteOrder - lanza DomainException si no existe el pedido")]
    public async Task DeleteOrder_WhenOrderNotFound_ThrowsDomainException()
    {
        // ARRANGE
        _orderRepoMock.Setup(r => r.GetByIdAsync(100)).ReturnsAsync((Order?)null);

        // ACT
        var act = () => _sut.DeleteOrderAsync(100);

        // ASSERT
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*100*no encontrado*");
    }

    [Fact(DisplayName = "9c. DeleteOrder - lanza excepción si el repositorio no pudo eliminar")]
    public async Task DeleteOrder_WhenRepositoryReturnsFalse_ThrowsInvalidOperationException()
    {
        // ARRANGE
        var existing = new Order { Id = 8, ProductId = 1, Quantity = 1 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
        _orderRepoMock.Setup(r => r.DeleteAsync(8)).ReturnsAsync(false);

        // ACT
        var act = () => _sut.DeleteOrderAsync(8);

        // ASSERT
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No se pudo eliminar el pedido.");
    }

    [Fact(DisplayName = "10a. TryCreateOrder - devuelve true cuando el repositorio crea el pedido")]
    public async Task TryCreateOrder_WhenRepositoryCreates_ReturnsTrueAndLogs()
    {
        // ARRANGE
        var order = new Order { ProductId = 1, Quantity = 2, TotalPrice = 3000m };
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ReturnsAsync(new Order { Id = 11 });

        // ACT
        var result = await _sut.TryCreateOrderAsync(order);

        // ASSERT
        result.Should().BeTrue();
        _orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once());
        _loggerMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.AtLeastOnce());
    }

    [Fact(DisplayName = "10b. TryCreateOrder - devuelve false y logea error si el repo falla")]
    public async Task TryCreateOrder_WhenRepositoryThrows_ReturnsFalseAndLogsError()
    {
        // ARRANGE
        var order = new Order { ProductId = 1, Quantity = 1 };
        _orderRepoMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                      .ThrowsAsync(new InvalidOperationException("DB error"));

        // ACT
        var result = await _sut.TryCreateOrderAsync(order);

        // ASSERT
        result.Should().BeFalse();
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception?>()), Times.Once());
    }

    [Fact(DisplayName = "11a. DemoFail - prueba que falla intencionalmente")]
    public void DemoFail_IntentionallyFails()
    {
        // Prueba creada intencionalmente para demostrar un fallo
        Assert.True(false, "Fallo intencional para demostración");
    }

    [Fact(DisplayName = "12a. UpdateOrder - actualiza pedido existente correctamente")]
    public async Task UpdateOrder_WhenOrderExists_ReturnsUpdatedOrder()
    {
        // ARRANGE
        var existing = new Order { Id = 20, ProductId = 1, Quantity = 1, TotalPrice = 1500m, Status = "Confirmed" };
        var updatedInput = new Order { Id = 20, ProductId = 1, Quantity = 3, TotalPrice = 4500m, Status = "Shipped" };

        _orderRepoMock.Setup(r => r.GetByIdAsync(20)).ReturnsAsync(existing);
        _orderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                      .ReturnsAsync((Order o) => o);

        // ACT
        var result = await _sut.UpdateOrderAsync(updatedInput);

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(20);
        result.Quantity.Should().Be(3);
        result.TotalPrice.Should().Be(4500m);
        result.Status.Should().Be("Shipped");
        _orderRepoMock.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Id == 20 && o.Quantity == 3)), Times.Once());
    }

    [Fact(DisplayName = "12b. UpdateOrder - lanza DomainException si no existe el pedido")]
    public async Task UpdateOrder_WhenOrderNotFound_ThrowsDomainException()
    {
        // ARRANGE
        var update = new Order { Id = 21, Quantity = 2 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(21)).ReturnsAsync((Order?)null);

        // ACT
        var act = () => _sut.UpdateOrderAsync(update);

        // ASSERT
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*21*no encontrado*");
    }

    [Fact(DisplayName = "12c. UpdateOrder - propaga excepción si el repositorio falla")]
    public async Task UpdateOrder_WhenRepositoryThrows_ExceptionPropagates()
    {
        // ARRANGE
        var existing = new Order { Id = 22, ProductId = 1, Quantity = 1 };
        var update = new Order { Id = 22, Quantity = 5 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(22)).ReturnsAsync(existing);
        _orderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>())).ThrowsAsync(new InvalidOperationException("DB update error"));

        // ACT
        var act = () => _sut.UpdateOrderAsync(update);

        // ASSERT
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB update error");
    }
}
