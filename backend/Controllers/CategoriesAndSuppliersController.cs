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
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cats = await _db.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.Products.Count))
            .ToListAsync();
        return Ok(cats);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest req)
    {
        var cat = new Category { Name = req.Name, Description = req.Description };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return Ok(cat);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateCategoryRequest req)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        cat.Name = req.Name;
        cat.Description = req.Description;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _db;
    public SuppliersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _db.Suppliers
            .Include(s => s.Products)
            .Select(s => new SupplierDto(s.Id, s.Name, s.ContactPerson, s.Email,
                s.Phone, s.Address, s.IsActive, s.Products.Count))
            .ToListAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var s = await _db.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == id);
        if (s == null) return NotFound();
        return Ok(new SupplierDto(s.Id, s.Name, s.ContactPerson, s.Email,
            s.Phone, s.Address, s.IsActive, s.Products.Count));
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSupplierRequest req)
    {
        var supplier = new Supplier
        {
            Name = req.Name, ContactPerson = req.ContactPerson,
            Email = req.Email, Phone = req.Phone, Address = req.Address
        };
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return Ok(supplier);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateSupplierRequest req)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();
        supplier.Name = req.Name; supplier.ContactPerson = req.ContactPerson;
        supplier.Email = req.Email; supplier.Phone = req.Phone; supplier.Address = req.Address;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();
        supplier.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
