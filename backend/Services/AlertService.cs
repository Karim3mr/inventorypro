using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Services;

public interface IAlertService
{
    Task CheckAndCreateAlertsAsync(Product product);
    Task ResolveAlertsForProductAsync(int productId);
}

public class AlertService : IAlertService
{
    private readonly AppDbContext _db;
    public AlertService(AppDbContext db) => _db = db;

    public async Task CheckAndCreateAlertsAsync(Product product)
    {
        // Resolve old alerts first
        await ResolveAlertsForProductAsync(product.Id);

        if (product.QuantityInStock == 0)
        {
            _db.Alerts.Add(new Alert
            {
                ProductId = product.Id,
                AlertType = "OUT_OF_STOCK",
                Message = $"'{product.Name}' (SKU: {product.SKU}) is out of stock."
            });
        }
        else if (product.QuantityInStock <= product.LowStockThreshold)
        {
            _db.Alerts.Add(new Alert
            {
                ProductId = product.Id,
                AlertType = "LOW_STOCK",
                Message = $"'{product.Name}' (SKU: {product.SKU}) is running low. " +
                          $"Current stock: {product.QuantityInStock} units."
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task ResolveAlertsForProductAsync(int productId)
    {
        var openAlerts = await _db.Alerts
            .Where(a => a.ProductId == productId && !a.IsResolved)
            .ToListAsync();

        foreach (var alert in openAlerts)
        {
            alert.IsResolved = true;
            alert.ResolvedAt = DateTime.UtcNow;
        }
    }
}
