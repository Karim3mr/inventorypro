namespace InventoryAPI.DTOs;

// ─── Auth ────────────────────────────────────────────────
public record LoginRequest(string Email, string Password);
public record RegisterRequest(string FullName, string Email, string Password, string Role);
public record AuthResponse(string Token, string FullName, string Email, string Role);

// ─── Category ────────────────────────────────────────────
public record CategoryDto(int Id, string Name, string? Description, int ProductCount);
public record CreateCategoryRequest(string Name, string? Description);

// ─── Supplier ────────────────────────────────────────────
public record SupplierDto(int Id, string Name, string ContactPerson, string Email,
    string Phone, string Address, bool IsActive, int ProductCount);
public record CreateSupplierRequest(string Name, string ContactPerson,
    string Email, string Phone, string Address);

// ─── Product ─────────────────────────────────────────────
public record ProductDto(int Id, string Name, string SKU, string? Barcode,
    string? Description, decimal Price, decimal CostPrice,
    int QuantityInStock, int LowStockThreshold,
    string CategoryName, string SupplierName,
    int CategoryId, int SupplierId,
    bool IsActive, DateTime CreatedAt);

public record CreateProductRequest(
    string Name, string SKU, string? Barcode, string? Description,
    decimal Price, decimal CostPrice, int QuantityInStock,
    int LowStockThreshold, int CategoryId, int SupplierId);

public record UpdateProductRequest(
    string Name, string? Barcode, string? Description,
    decimal Price, decimal CostPrice, int LowStockThreshold,
    int CategoryId, int SupplierId, bool IsActive);

// ─── Stock ───────────────────────────────────────────────
public record StockTransactionDto(int Id, string ProductName, string SKU,
    string TransactionType, int Quantity, int QuantityBefore, int QuantityAfter,
    string? Notes, string UserName, DateTime CreatedAt);

public record CreateStockTransactionRequest(
    int ProductId, string TransactionType, int Quantity, string? Notes);

// ─── Alert ───────────────────────────────────────────────
public record AlertDto(int Id, string ProductName, string SKU,
    string AlertType, string Message, bool IsResolved, DateTime CreatedAt);

// ─── Reports ─────────────────────────────────────────────
public record DashboardStats(
    int TotalProducts, int TotalSuppliers, int TotalCategories,
    int LowStockCount, int OutOfStockCount, int ActiveAlerts,
    decimal TotalInventoryValue);

public record StockMovementReport(string ProductName, string SKU,
    int TotalIn, int TotalOut, int CurrentStock);

public record TopProductsReport(string ProductName, string SKU,
    int TotalMovement, decimal TotalValue);
