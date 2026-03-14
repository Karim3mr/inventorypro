using InventoryAPI.Data;
using InventoryAPI.DTOs;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] bool? lowStock)
    {
        var q = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(p => p.Name.Contains(search) || p.SKU.Contains(search) ||
                              (p.Barcode != null && p.Barcode.Contains(search)));

        if (categoryId.HasValue)
            q = q.Where(p => p.CategoryId == categoryId);

        if (lowStock == true)
            q = q.Where(p => p.QuantityInStock <= p.LowStockThreshold);

        var products = await q
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(p.Id, p.Name, p.SKU, p.Barcode, p.Description,
                p.Price, p.CostPrice, p.QuantityInStock, p.LowStockThreshold,
                p.Category.Name, p.Supplier.Name, p.CategoryId, p.SupplierId,
                p.IsActive, p.CreatedAt))
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _db.Products
            .Include(p => p.Category).Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (p == null) return NotFound();
        return Ok(new ProductDto(p.Id, p.Name, p.SKU, p.Barcode, p.Description,
            p.Price, p.CostPrice, p.QuantityInStock, p.LowStockThreshold,
            p.Category.Name, p.Supplier.Name, p.CategoryId, p.SupplierId,
            p.IsActive, p.CreatedAt));
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest req)
    {
        if (await _db.Products.AnyAsync(p => p.SKU == req.SKU))
            return BadRequest(new { message = "SKU already exists." });

        var product = new Product
        {
            Name = req.Name, SKU = req.SKU, Barcode = req.Barcode,
            Description = req.Description, Price = req.Price,
            CostPrice = req.CostPrice, QuantityInStock = req.QuantityInStock,
            LowStockThreshold = req.LowStockThreshold,
            CategoryId = req.CategoryId, SupplierId = req.SupplierId
        };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest req)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Name = req.Name;
        product.Barcode = req.Barcode;
        product.Description = req.Description;
        product.Price = req.Price;
        product.CostPrice = req.CostPrice;
        product.LowStockThreshold = req.LowStockThreshold;
        product.CategoryId = req.CategoryId;
        product.SupplierId = req.SupplierId;
        product.IsActive = req.IsActive;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        product.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/barcode")]
    public IActionResult GetBarcode(int id)
    {
        // Returns barcode value — frontend renders it using react-barcode
        var product = _db.Products.Find(id);
        if (product == null) return NotFound();
        return Ok(new { barcode = product.Barcode ?? product.SKU });
    }
}
