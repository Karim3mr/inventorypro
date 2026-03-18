using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>()
            .Property(p => p.CostPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU).IsUnique();

        // Users
        modelBuilder.Entity<User>().HasData(
            new User { Id=1, FullName="System Admin",  Email="admin@inventory.com",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role="Admin",   IsActive=true, CreatedAt=new DateTime(2024,1,1,0,0,0,DateTimeKind.Utc) },
            new User { Id=2, FullName="Sara Manager",  Email="sara@inventory.com",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                Role="Manager", IsActive=true, CreatedAt=new DateTime(2024,1,5,0,0,0,DateTimeKind.Utc) },
            new User { Id=3, FullName="Omar Staff",    Email="omar@inventory.com",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                Role="Staff",   IsActive=true, CreatedAt=new DateTime(2024,1,10,0,0,0,DateTimeKind.Utc) }
        );

        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id=1, Name="Electronics",     Description="Electronic devices, gadgets and components" },
            new Category { Id=2, Name="Office Supplies", Description="Stationery, paper and office essentials" },
            new Category { Id=3, Name="Furniture",       Description="Office and warehouse furniture" },
            new Category { Id=4, Name="Networking",      Description="Routers, switches, cables and networking gear" },
            new Category { Id=5, Name="Accessories",     Description="Peripheral devices and accessories" }
        );

        // Suppliers
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id=1, Name="TechWorld Trading",  ContactPerson="Ahmed Hassan",
                Email="ahmed@techworld.com",    Phone="+20-100-123-4567",
                Address="10 Nasr City, Cairo, Egypt",  IsActive=true, CreatedAt=new DateTime(2024,1,2,0,0,0,DateTimeKind.Utc) },
            new Supplier { Id=2, Name="Office Hub Co.",     ContactPerson="Mona Khalil",
                Email="mona@officehub.com",     Phone="+20-112-987-6543",
                Address="55 Dokki St, Giza, Egypt",    IsActive=true, CreatedAt=new DateTime(2024,1,3,0,0,0,DateTimeKind.Utc) },
            new Supplier { Id=3, Name="NetGear Solutions",  ContactPerson="Karim Samir",
                Email="karim@netgear-eg.com",   Phone="+20-115-456-7890",
                Address="22 Maadi, Cairo, Egypt",      IsActive=true, CreatedAt=new DateTime(2024,1,4,0,0,0,DateTimeKind.Utc) },
            new Supplier { Id=4, Name="FurniPro Egypt",     ContactPerson="Layla Ibrahim",
                Email="layla@furnipro.com",     Phone="+20-101-321-6549",
                Address="8 Heliopolis, Cairo, Egypt",  IsActive=true, CreatedAt=new DateTime(2024,1,5,0,0,0,DateTimeKind.Utc) }
        );

        // Products
        var cd = DateTimeKind.Utc;
        modelBuilder.Entity<Product>().HasData(
            new Product { Id=1,  Name="Dell Laptop 15\"",        SKU="EL-001", Barcode="8901234567890", Price=1299.99m, CostPrice=980.00m,  QuantityInStock=25, LowStockThreshold=5,  CategoryId=1, SupplierId=1, IsActive=true, Description="Dell Inspiron 15, Intel i7, 16GB RAM, 512GB SSD",               CreatedAt=new DateTime(2024,1,10,0,0,0,cd) },
            new Product { Id=2,  Name="HP LaserJet Printer",     SKU="EL-002", Barcode="8901234567891", Price=349.99m,  CostPrice=240.00m,  QuantityInStock=8,  LowStockThreshold=5,  CategoryId=1, SupplierId=1, IsActive=true, Description="HP LaserJet Pro M404dn, 40 ppm",                                CreatedAt=new DateTime(2024,1,11,0,0,0,cd) },
            new Product { Id=3,  Name="Samsung Monitor 27\"",    SKU="EL-003", Barcode="8901234567892", Price=429.99m,  CostPrice=310.00m,  QuantityInStock=14, LowStockThreshold=4,  CategoryId=1, SupplierId=1, IsActive=true, Description="Samsung 27\" 4K UHD IPS Monitor",                               CreatedAt=new DateTime(2024,1,12,0,0,0,cd) },
            new Product { Id=4,  Name="Mechanical Keyboard",     SKU="AC-001", Barcode="8901234567893", Price=89.99m,   CostPrice=55.00m,   QuantityInStock=3,  LowStockThreshold=5,  CategoryId=5, SupplierId=1, IsActive=true, Description="Logitech MX Keys Mechanical, Wireless",                         CreatedAt=new DateTime(2024,1,13,0,0,0,cd) },
            new Product { Id=5,  Name="Wireless Mouse",          SKU="AC-002", Barcode="8901234567894", Price=49.99m,   CostPrice=28.00m,   QuantityInStock=0,  LowStockThreshold=5,  CategoryId=5, SupplierId=1, IsActive=true, Description="Logitech MX Master 3 Wireless Mouse",                           CreatedAt=new DateTime(2024,1,14,0,0,0,cd) },
            new Product { Id=6,  Name="Cisco Switch 24-Port",    SKU="NW-001", Barcode="8901234567895", Price=599.99m,  CostPrice=420.00m,  QuantityInStock=6,  LowStockThreshold=3,  CategoryId=4, SupplierId=3, IsActive=true, Description="Cisco Catalyst 2960 24-Port Gigabit Switch",                    CreatedAt=new DateTime(2024,1,15,0,0,0,cd) },
            new Product { Id=7,  Name="TP-Link WiFi Router",     SKU="NW-002", Barcode="8901234567896", Price=129.99m,  CostPrice=78.00m,   QuantityInStock=18, LowStockThreshold=5,  CategoryId=4, SupplierId=3, IsActive=true, Description="TP-Link AX3000 Dual-Band WiFi 6 Router",                        CreatedAt=new DateTime(2024,1,16,0,0,0,cd) },
            new Product { Id=8,  Name="Cat6 Ethernet Cable 50m", SKU="NW-003", Barcode="8901234567897", Price=24.99m,   CostPrice=12.00m,   QuantityInStock=80, LowStockThreshold=20, CategoryId=4, SupplierId=3, IsActive=true, Description="Cat6 UTP 50m Ethernet Cable, 10Gbps",                           CreatedAt=new DateTime(2024,1,17,0,0,0,cd) },
            new Product { Id=9,  Name="A4 Paper Ream 500 Sheets",SKU="OF-001", Barcode="8901234567898", Price=8.99m,    CostPrice=5.00m,    QuantityInStock=200,LowStockThreshold=30, CategoryId=2, SupplierId=2, IsActive=true, Description="Navigator A4 80gsm White Paper, 500 sheets",                    CreatedAt=new DateTime(2024,1,18,0,0,0,cd) },
            new Product { Id=10, Name="Stapler Heavy Duty",      SKU="OF-002", Barcode="8901234567899", Price=19.99m,   CostPrice=10.00m,   QuantityInStock=4,  LowStockThreshold=5,  CategoryId=2, SupplierId=2, IsActive=true, Description="Rapid Heavy Duty Stapler, 210 sheet capacity",                  CreatedAt=new DateTime(2024,1,19,0,0,0,cd) },
            new Product { Id=11, Name="Whiteboard Markers Set",  SKU="OF-003", Barcode="8901234567900", Price=12.99m,   CostPrice=6.50m,    QuantityInStock=45, LowStockThreshold=10, CategoryId=2, SupplierId=2, IsActive=true, Description="Expo Dry-Erase Markers, 8-color set",                           CreatedAt=new DateTime(2024,1,20,0,0,0,cd) },
            new Product { Id=12, Name="Ergonomic Office Chair",  SKU="FN-001", Barcode="8901234567901", Price=399.99m,  CostPrice=270.00m,  QuantityInStock=10, LowStockThreshold=3,  CategoryId=3, SupplierId=4, IsActive=true, Description="Herman Miller Aeron Style, Lumbar Support, Adjustable",         CreatedAt=new DateTime(2024,1,21,0,0,0,cd) },
            new Product { Id=13, Name="Standing Desk 160cm",     SKU="FN-002", Barcode="8901234567902", Price=549.99m,  CostPrice=380.00m,  QuantityInStock=7,  LowStockThreshold=2,  CategoryId=3, SupplierId=4, IsActive=true, Description="Electric Height-Adjustable Standing Desk, 160x80cm",            CreatedAt=new DateTime(2024,1,22,0,0,0,cd) },
            new Product { Id=14, Name="4-Drawer Filing Cabinet", SKU="FN-003", Barcode="8901234567903", Price=229.99m,  CostPrice=155.00m,  QuantityInStock=2,  LowStockThreshold=3,  CategoryId=3, SupplierId=4, IsActive=true, Description="Steel 4-Drawer Vertical Filing Cabinet with Lock",              CreatedAt=new DateTime(2024,1,23,0,0,0,cd) },
            new Product { Id=15, Name="USB-C Hub 7-in-1",        SKU="AC-003", Barcode="8901234567904", Price=59.99m,   CostPrice=32.00m,   QuantityInStock=22, LowStockThreshold=8,  CategoryId=5, SupplierId=1, IsActive=true, Description="USB-C Hub with HDMI, 3x USB-A, SD Card, 100W PD",               CreatedAt=new DateTime(2024,1,24,0,0,0,cd) }
        );

        // Stock Transactions
        var t = new DateTime(2024,2,1,9,0,0,DateTimeKind.Utc);
        modelBuilder.Entity<StockTransaction>().HasData(
            // Initial IN
            new StockTransaction { Id=1,  ProductId=1,  TransactionType="IN",         Quantity=30,  QuantityBefore=0,   QuantityAfter=30,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=2,  ProductId=2,  TransactionType="IN",         Quantity=15,  QuantityBefore=0,   QuantityAfter=15,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=3,  ProductId=3,  TransactionType="IN",         Quantity=20,  QuantityBefore=0,   QuantityAfter=20,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=4,  ProductId=4,  TransactionType="IN",         Quantity=15,  QuantityBefore=0,   QuantityAfter=15,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=5,  ProductId=5,  TransactionType="IN",         Quantity=20,  QuantityBefore=0,   QuantityAfter=20,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=6,  ProductId=6,  TransactionType="IN",         Quantity=10,  QuantityBefore=0,   QuantityAfter=10,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=7,  ProductId=7,  TransactionType="IN",         Quantity=25,  QuantityBefore=0,   QuantityAfter=25,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=8,  ProductId=8,  TransactionType="IN",         Quantity=100, QuantityBefore=0,   QuantityAfter=100, UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=9,  ProductId=9,  TransactionType="IN",         Quantity=300, QuantityBefore=0,   QuantityAfter=300, UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=10, ProductId=10, TransactionType="IN",         Quantity=20,  QuantityBefore=0,   QuantityAfter=20,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=11, ProductId=11, TransactionType="IN",         Quantity=60,  QuantityBefore=0,   QuantityAfter=60,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=12, ProductId=12, TransactionType="IN",         Quantity=15,  QuantityBefore=0,   QuantityAfter=15,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=13, ProductId=13, TransactionType="IN",         Quantity=10,  QuantityBefore=0,   QuantityAfter=10,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=14, ProductId=14, TransactionType="IN",         Quantity=8,   QuantityBefore=0,   QuantityAfter=8,   UserId=1, Notes="Initial stock received", CreatedAt=t },
            new StockTransaction { Id=15, ProductId=15, TransactionType="IN",         Quantity=30,  QuantityBefore=0,   QuantityAfter=30,  UserId=1, Notes="Initial stock received", CreatedAt=t },
            // Feb OUT
            new StockTransaction { Id=16, ProductId=1,  TransactionType="OUT",        Quantity=3,   QuantityBefore=30,  QuantityAfter=27,  UserId=2, Notes="Sold to Dept. A",               CreatedAt=t.AddDays(3) },
            new StockTransaction { Id=17, ProductId=5,  TransactionType="OUT",        Quantity=8,   QuantityBefore=20,  QuantityAfter=12,  UserId=2, Notes="Sales order #1022",              CreatedAt=t.AddDays(3) },
            new StockTransaction { Id=18, ProductId=9,  TransactionType="OUT",        Quantity=50,  QuantityBefore=300, QuantityAfter=250, UserId=3, Notes="Office weekly supply",            CreatedAt=t.AddDays(4) },
            new StockTransaction { Id=19, ProductId=7,  TransactionType="OUT",        Quantity=4,   QuantityBefore=25,  QuantityAfter=21,  UserId=2, Notes="Sales order #1023",              CreatedAt=t.AddDays(5) },
            new StockTransaction { Id=20, ProductId=3,  TransactionType="OUT",        Quantity=3,   QuantityBefore=20,  QuantityAfter=17,  UserId=2, Notes="Sales order #1024",              CreatedAt=t.AddDays(6) },
            new StockTransaction { Id=21, ProductId=12, TransactionType="OUT",        Quantity=2,   QuantityBefore=15,  QuantityAfter=13,  UserId=2, Notes="New office setup",               CreatedAt=t.AddDays(7) },
            new StockTransaction { Id=22, ProductId=13, TransactionType="OUT",        Quantity=1,   QuantityBefore=10,  QuantityAfter=9,   UserId=2, Notes="CEO office upgrade",             CreatedAt=t.AddDays(8) },
            new StockTransaction { Id=23, ProductId=15, TransactionType="OUT",        Quantity=5,   QuantityBefore=30,  QuantityAfter=25,  UserId=3, Notes="Sales order #1025",              CreatedAt=t.AddDays(9) },
            new StockTransaction { Id=24, ProductId=2,  TransactionType="OUT",        Quantity=3,   QuantityBefore=15,  QuantityAfter=12,  UserId=2, Notes="Sold to IT dept",                CreatedAt=t.AddDays(10) },
            new StockTransaction { Id=25, ProductId=11, TransactionType="OUT",        Quantity=10,  QuantityBefore=60,  QuantityAfter=50,  UserId=3, Notes="Training room supply",           CreatedAt=t.AddDays(10) },
            // March IN
            new StockTransaction { Id=26, ProductId=5,  TransactionType="IN",         Quantity=10,  QuantityBefore=12,  QuantityAfter=22,  UserId=1, Notes="Restock from TechWorld",         CreatedAt=t.AddDays(30) },
            new StockTransaction { Id=27, ProductId=9,  TransactionType="IN",         Quantity=100, QuantityBefore=250, QuantityAfter=350, UserId=1, Notes="Monthly restock",                CreatedAt=t.AddDays(31) },
            new StockTransaction { Id=28, ProductId=4,  TransactionType="IN",         Quantity=10,  QuantityBefore=15,  QuantityAfter=25,  UserId=1, Notes="Restock order",                  CreatedAt=t.AddDays(32) },
            new StockTransaction { Id=29, ProductId=8,  TransactionType="IN",         Quantity=50,  QuantityBefore=100, QuantityAfter=150, UserId=1, Notes="Bulk cable purchase",             CreatedAt=t.AddDays(32) },
            // March OUT
            new StockTransaction { Id=30, ProductId=1,  TransactionType="OUT",        Quantity=2,   QuantityBefore=27,  QuantityAfter=25,  UserId=2, Notes="Sales order #1030",              CreatedAt=t.AddDays(35) },
            new StockTransaction { Id=31, ProductId=3,  TransactionType="OUT",        Quantity=3,   QuantityBefore=17,  QuantityAfter=14,  UserId=2, Notes="Sales order #1031",              CreatedAt=t.AddDays(36) },
            new StockTransaction { Id=32, ProductId=5,  TransactionType="OUT",        Quantity=12,  QuantityBefore=22,  QuantityAfter=10,  UserId=2, Notes="Sales order #1032",              CreatedAt=t.AddDays(37) },
            new StockTransaction { Id=33, ProductId=12, TransactionType="OUT",        Quantity=3,   QuantityBefore=13,  QuantityAfter=10,  UserId=2, Notes="3rd floor office fit-out",       CreatedAt=t.AddDays(38) },
            new StockTransaction { Id=34, ProductId=9,  TransactionType="OUT",        Quantity=100, QuantityBefore=350, QuantityAfter=250, UserId=3, Notes="Monthly office supply",           CreatedAt=t.AddDays(38) },
            new StockTransaction { Id=35, ProductId=6,  TransactionType="OUT",        Quantity=2,   QuantityBefore=10,  QuantityAfter=8,   UserId=2, Notes="Network upgrade project",        CreatedAt=t.AddDays(39) },
            new StockTransaction { Id=36, ProductId=7,  TransactionType="OUT",        Quantity=3,   QuantityBefore=21,  QuantityAfter=18,  UserId=2, Notes="Sales order #1035",              CreatedAt=t.AddDays(40) },
            new StockTransaction { Id=37, ProductId=2,  TransactionType="OUT",        Quantity=4,   QuantityBefore=12,  QuantityAfter=8,   UserId=2, Notes="Sales order #1036",              CreatedAt=t.AddDays(41) },
            new StockTransaction { Id=38, ProductId=10, TransactionType="OUT",        Quantity=8,   QuantityBefore=20,  QuantityAfter=12,  UserId=3, Notes="Stationery replenishment",       CreatedAt=t.AddDays(42) },
            new StockTransaction { Id=39, ProductId=15, TransactionType="OUT",        Quantity=8,   QuantityBefore=25,  QuantityAfter=17,  UserId=2, Notes="Sales order #1037",              CreatedAt=t.AddDays(43) },
            // April OUT (heavy — brings to low/zero stock)
            new StockTransaction { Id=40, ProductId=5,  TransactionType="OUT",        Quantity=10,  QuantityBefore=10,  QuantityAfter=0,   UserId=2, Notes="Sales order #1040 - cleared stock", CreatedAt=t.AddDays(65) },
            new StockTransaction { Id=41, ProductId=4,  TransactionType="OUT",        Quantity=22,  QuantityBefore=25,  QuantityAfter=3,   UserId=2, Notes="Sales order #1041",              CreatedAt=t.AddDays(66) },
            new StockTransaction { Id=42, ProductId=10, TransactionType="OUT",        Quantity=8,   QuantityBefore=12,  QuantityAfter=4,   UserId=3, Notes="Sales order #1042",              CreatedAt=t.AddDays(67) },
            new StockTransaction { Id=43, ProductId=14, TransactionType="OUT",        Quantity=6,   QuantityBefore=8,   QuantityAfter=2,   UserId=2, Notes="Sales order #1043",              CreatedAt=t.AddDays(68) },
            new StockTransaction { Id=44, ProductId=6,  TransactionType="OUT",        Quantity=2,   QuantityBefore=8,   QuantityAfter=6,   UserId=2, Notes="Network expansion",              CreatedAt=t.AddDays(70) },
            new StockTransaction { Id=45, ProductId=8,  TransactionType="OUT",        Quantity=70,  QuantityBefore=150, QuantityAfter=80,  UserId=3, Notes="Cabling project bulk",           CreatedAt=t.AddDays(70) },
            new StockTransaction { Id=46, ProductId=9,  TransactionType="OUT",        Quantity=50,  QuantityBefore=250, QuantityAfter=200, UserId=3, Notes="Monthly supply",                 CreatedAt=t.AddDays(71) },
            new StockTransaction { Id=47, ProductId=11, TransactionType="OUT",        Quantity=5,   QuantityBefore=50,  QuantityAfter=45,  UserId=3, Notes="Training supply",                CreatedAt=t.AddDays(72) }
        );

        // Alerts
        modelBuilder.Entity<Alert>().HasData(
            new Alert { Id=1, ProductId=5,  AlertType="OUT_OF_STOCK", Message="'Wireless Mouse' (SKU: AC-002) is out of stock.",                                           IsResolved=false, CreatedAt=t.AddDays(65) },
            new Alert { Id=2, ProductId=4,  AlertType="LOW_STOCK",    Message="'Mechanical Keyboard' (SKU: AC-001) is running low. Current stock: 3 units.",              IsResolved=false, CreatedAt=t.AddDays(66) },
            new Alert { Id=3, ProductId=10, AlertType="LOW_STOCK",    Message="'Stapler Heavy Duty' (SKU: OF-002) is running low. Current stock: 4 units.",               IsResolved=false, CreatedAt=t.AddDays(67) },
            new Alert { Id=4, ProductId=14, AlertType="LOW_STOCK",    Message="'4-Drawer Filing Cabinet' (SKU: FN-003) is running low. Current stock: 2 units.",          IsResolved=false, CreatedAt=t.AddDays(68) },
            new Alert { Id=5, ProductId=5,  AlertType="LOW_STOCK",    Message="'Wireless Mouse' (SKU: AC-002) was running low. Stock: 10 units.",                         IsResolved=true,  CreatedAt=t.AddDays(37), ResolvedAt=t.AddDays(65) },
            new Alert { Id=6, ProductId=9,  AlertType="LOW_STOCK",    Message="'A4 Paper Ream' (SKU: OF-001) was running low.",                                           IsResolved=true,  CreatedAt=t.AddDays(25), ResolvedAt=t.AddDays(31) }
        );
    }
}
