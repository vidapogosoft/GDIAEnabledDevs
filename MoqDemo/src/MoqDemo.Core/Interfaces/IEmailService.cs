namespace MoqDemo.Core.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(string email, int orderId);
    Task SendLowStockAlertAsync(string adminEmail, int productId, int remainingStock);
}
