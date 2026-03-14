using InventoryAPI.Data;
using InventoryAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ReportsController(AppDbContext db) => _db = db;

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var products = await _db.Products.ToListAsync();
        var activeAlerts = await _db.Alerts.CountAsync(a => !a.IsResolved);
        var totalValue = products.Sum(p => p.Price * p.QuantityInStock);

        return Ok(new DashboardStats(
            TotalProducts: products.Count,
            TotalSuppliers: await _db.Suppliers.CountAsync(s => s.IsActive),
            TotalCategories: await _db.Categories.CountAsync(),
            LowStockCount: products.Count(p => p.QuantityInStock > 0 && p.QuantityInStock <= p.LowStockThreshold),
            OutOfStockCount: products.Count(p => p.QuantityInStock == 0),
            ActiveAlerts: activeAlerts,
            TotalInventoryValue: totalValue
        ));
    }

    [HttpGet("stock-movement")]
    public async Task<IActionResult> StockMovement(
        [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var q = _db.StockTransactions
            .Include(t => t.Product)
            .AsQueryable();

        if (from.HasValue) q = q.Where(t => t.CreatedAt >= from);
        if (to.HasValue) q = q.Where(t => t.CreatedAt <= to);

        var report = await q
            .GroupBy(t => new { t.ProductId, t.Product.Name, t.Product.SKU })
            .Select(g => new StockMovementReport(
                g.Key.Name, g.Key.SKU,
                g.Where(t => t.TransactionType == "IN").Sum(t => t.Quantity),
                g.Where(t => t.TransactionType == "OUT").Sum(t => t.Quantity),
                g.OrderByDescending(t => t.CreatedAt).First().QuantityAfter
            ))
            .ToListAsync();

        return Ok(report);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> TopProducts()
    {
        var report = await _db.StockTransactions
            .Include(t => t.Product)
            .GroupBy(t => new { t.ProductId, t.Product.Name, t.Product.SKU, t.Product.Price })
            .Select(g => new TopProductsReport(
                g.Key.Name, g.Key.SKU,
                g.Sum(t => t.Quantity),
                g.Sum(t => t.Quantity) * g.Key.Price
            ))
            .OrderByDescending(r => r.TotalMovement)
            .Take(10)
            .ToListAsync();

        return Ok(report);
    }

    [HttpGet("inventory-value")]
    public async Task<IActionResult> InventoryValue()
    {
        var data = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .GroupBy(p => p.Category.Name)
            .Select(g => new
            {
                Category = g.Key,
                TotalValue = g.Sum(p => p.Price * p.QuantityInStock),
                ProductCount = g.Count()
            })
            .ToListAsync();

        return Ok(data);
    }
}
