using System.Security.Claims;
using InventoryAPI.Data;
using InventoryAPI.DTOs;
using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IAlertService _alertService;
    public StockController(AppDbContext db, IAlertService alertService)
    {
        _db = db;
        _alertService = alertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] int? productId,
        [FromQuery] string? type,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var q = _db.StockTransactions
            .Include(t => t.Product)
            .Include(t => t.User)
            .AsQueryable();

        if (productId.HasValue) q = q.Where(t => t.ProductId == productId);
        if (!string.IsNullOrEmpty(type)) q = q.Where(t => t.TransactionType == type);
        if (from.HasValue) q = q.Where(t => t.CreatedAt >= from);
        if (to.HasValue) q = q.Where(t => t.CreatedAt <= to);

        var results = await q.OrderByDescending(t => t.CreatedAt)
            .Take(200)
            .Select(t => new StockTransactionDto(
                t.Id, t.Product.Name, t.Product.SKU,
                t.TransactionType, t.Quantity,
                t.QuantityBefore, t.QuantityAfter,
                t.Notes, t.User.FullName, t.CreatedAt))
            .ToListAsync();

        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(CreateStockTransactionRequest req)
    {
        var product = await _db.Products.FindAsync(req.ProductId);
        if (product == null) return NotFound(new { message = "Product not found." });

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        int qBefore = product.QuantityInStock;
        int qAfter = req.TransactionType switch
        {
            "IN" => qBefore + req.Quantity,
            "OUT" => qBefore - req.Quantity,
            "ADJUSTMENT" => req.Quantity,
            _ => throw new ArgumentException("Invalid type")
        };

        if (qAfter < 0)
            return BadRequest(new { message = "Insufficient stock." });

        product.QuantityInStock = qAfter;

        _db.StockTransactions.Add(new StockTransaction
        {
            ProductId = req.ProductId,
            TransactionType = req.TransactionType,
            Quantity = req.Quantity,
            QuantityBefore = qBefore,
            QuantityAfter = qAfter,
            Notes = req.Notes,
            UserId = userId
        });

        await _db.SaveChangesAsync();
        await _alertService.CheckAndCreateAlertsAsync(product);

        return Ok(new { message = "Stock updated.", newQuantity = qAfter });
    }
}

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AlertsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAlerts([FromQuery] bool? resolved)
    {
        var q = _db.Alerts.Include(a => a.Product).AsQueryable();
        if (resolved.HasValue) q = q.Where(a => a.IsResolved == resolved);

        var alerts = await q.OrderByDescending(a => a.CreatedAt)
            .Select(a => new AlertDto(
                a.Id, a.Product.Name, a.Product.SKU,
                a.AlertType, a.Message, a.IsResolved, a.CreatedAt))
            .ToListAsync();

        return Ok(alerts);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPatch("{id}/resolve")]
    public async Task<IActionResult> Resolve(int id)
    {
        var alert = await _db.Alerts.FindAsync(id);
        if (alert == null) return NotFound();
        alert.IsResolved = true;
        alert.ResolvedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
