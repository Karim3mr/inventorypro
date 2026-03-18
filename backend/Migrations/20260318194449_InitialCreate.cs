using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QuantityBefore = table.Column<int>(type: "int", nullable: false),
                    QuantityAfter = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Electronic devices, gadgets and components", "Electronics" },
                    { 2, "Stationery, paper and office essentials", "Office Supplies" },
                    { 3, "Office and warehouse furniture", "Furniture" },
                    { 4, "Routers, switches, cables and networking gear", "Networking" },
                    { 5, "Peripheral devices and accessories", "Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "ContactPerson", "CreatedAt", "Email", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "10 Nasr City, Cairo, Egypt", "Ahmed Hassan", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "ahmed@techworld.com", true, "TechWorld Trading", "+20-100-123-4567" },
                    { 2, "55 Dokki St, Giza, Egypt", "Mona Khalil", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "mona@officehub.com", true, "Office Hub Co.", "+20-112-987-6543" },
                    { 3, "22 Maadi, Cairo, Egypt", "Karim Samir", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "karim@netgear-eg.com", true, "NetGear Solutions", "+20-115-456-7890" },
                    { 4, "8 Heliopolis, Cairo, Egypt", "Layla Ibrahim", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "layla@furnipro.com", true, "FurniPro Egypt", "+20-101-321-6549" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@inventory.com", "System Admin", true, "$2a$11$JYSOpyPlfpyzJFdzu84gj.pgBxawBpCq4f3UpAxn5ZvgH2PXX9uLm", "Admin" },
                    { 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "sara@inventory.com", "Sara Manager", true, "$2a$11$NUfjsYxWhFYiBYsboOKB.u9YeRajBTeI0R3TjEdRob.HYSkTH.tJS", "Manager" },
                    { 3, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "omar@inventory.com", "Omar Staff", true, "$2a$11$uQcjUfoNO/ILkIYghoCzJOVCDq4ss1.UkXORaEIzEb7kqkmYYaSPq", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "CategoryId", "CostPrice", "CreatedAt", "Description", "ImageUrl", "IsActive", "LowStockThreshold", "Name", "Price", "QuantityInStock", "SKU", "SupplierId" },
                values: new object[,]
                {
                    { 1, "8901234567890", 1, 980.00m, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Dell Inspiron 15, Intel i7, 16GB RAM, 512GB SSD", null, true, 5, "Dell Laptop 15\"", 1299.99m, 25, "EL-001", 1 },
                    { 2, "8901234567891", 1, 240.00m, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "HP LaserJet Pro M404dn, 40 ppm", null, true, 5, "HP LaserJet Printer", 349.99m, 8, "EL-002", 1 },
                    { 3, "8901234567892", 1, 310.00m, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Samsung 27\" 4K UHD IPS Monitor", null, true, 4, "Samsung Monitor 27\"", 429.99m, 14, "EL-003", 1 },
                    { 4, "8901234567893", 5, 55.00m, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Logitech MX Keys Mechanical, Wireless", null, true, 5, "Mechanical Keyboard", 89.99m, 3, "AC-001", 1 },
                    { 5, "8901234567894", 5, 28.00m, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Logitech MX Master 3 Wireless Mouse", null, true, 5, "Wireless Mouse", 49.99m, 0, "AC-002", 1 },
                    { 6, "8901234567895", 4, 420.00m, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Cisco Catalyst 2960 24-Port Gigabit Switch", null, true, 3, "Cisco Switch 24-Port", 599.99m, 6, "NW-001", 3 },
                    { 7, "8901234567896", 4, 78.00m, new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "TP-Link AX3000 Dual-Band WiFi 6 Router", null, true, 5, "TP-Link WiFi Router", 129.99m, 18, "NW-002", 3 },
                    { 8, "8901234567897", 4, 12.00m, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Cat6 UTP 50m Ethernet Cable, 10Gbps", null, true, 20, "Cat6 Ethernet Cable 50m", 24.99m, 80, "NW-003", 3 },
                    { 9, "8901234567898", 2, 5.00m, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Navigator A4 80gsm White Paper, 500 sheets", null, true, 30, "A4 Paper Ream 500 Sheets", 8.99m, 200, "OF-001", 2 },
                    { 10, "8901234567899", 2, 10.00m, new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Rapid Heavy Duty Stapler, 210 sheet capacity", null, true, 5, "Stapler Heavy Duty", 19.99m, 4, "OF-002", 2 },
                    { 11, "8901234567900", 2, 6.50m, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Expo Dry-Erase Markers, 8-color set", null, true, 10, "Whiteboard Markers Set", 12.99m, 45, "OF-003", 2 },
                    { 12, "8901234567901", 3, 270.00m, new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Herman Miller Aeron Style, Lumbar Support, Adjustable", null, true, 3, "Ergonomic Office Chair", 399.99m, 10, "FN-001", 4 },
                    { 13, "8901234567902", 3, 380.00m, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Electric Height-Adjustable Standing Desk, 160x80cm", null, true, 2, "Standing Desk 160cm", 549.99m, 7, "FN-002", 4 },
                    { 14, "8901234567903", 3, 155.00m, new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Steel 4-Drawer Vertical Filing Cabinet with Lock", null, true, 3, "4-Drawer Filing Cabinet", 229.99m, 2, "FN-003", 4 },
                    { 15, "8901234567904", 5, 32.00m, new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), "USB-C Hub with HDMI, 3x USB-A, SD Card, 100W PD", null, true, 8, "USB-C Hub 7-in-1", 59.99m, 22, "AC-003", 1 }
                });

            migrationBuilder.InsertData(
                table: "Alerts",
                columns: new[] { "Id", "AlertType", "CreatedAt", "IsResolved", "Message", "ProductId", "ResolvedAt" },
                values: new object[,]
                {
                    { 1, "OUT_OF_STOCK", new DateTime(2024, 4, 6, 9, 0, 0, 0, DateTimeKind.Utc), false, "'Wireless Mouse' (SKU: AC-002) is out of stock.", 5, null },
                    { 2, "LOW_STOCK", new DateTime(2024, 4, 7, 9, 0, 0, 0, DateTimeKind.Utc), false, "'Mechanical Keyboard' (SKU: AC-001) is running low. Current stock: 3 units.", 4, null },
                    { 3, "LOW_STOCK", new DateTime(2024, 4, 8, 9, 0, 0, 0, DateTimeKind.Utc), false, "'Stapler Heavy Duty' (SKU: OF-002) is running low. Current stock: 4 units.", 10, null },
                    { 4, "LOW_STOCK", new DateTime(2024, 4, 9, 9, 0, 0, 0, DateTimeKind.Utc), false, "'4-Drawer Filing Cabinet' (SKU: FN-003) is running low. Current stock: 2 units.", 14, null },
                    { 5, "LOW_STOCK", new DateTime(2024, 3, 9, 9, 0, 0, 0, DateTimeKind.Utc), true, "'Wireless Mouse' (SKU: AC-002) was running low. Stock: 10 units.", 5, new DateTime(2024, 4, 6, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "LOW_STOCK", new DateTime(2024, 2, 26, 9, 0, 0, 0, DateTimeKind.Utc), true, "'A4 Paper Ream' (SKU: OF-001) was running low.", 9, new DateTime(2024, 3, 3, 9, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "StockTransactions",
                columns: new[] { "Id", "CreatedAt", "Notes", "ProductId", "Quantity", "QuantityAfter", "QuantityBefore", "TransactionType", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 1, 30, 30, 0, "IN", 1 },
                    { 2, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 2, 15, 15, 0, "IN", 1 },
                    { 3, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 3, 20, 20, 0, "IN", 1 },
                    { 4, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 4, 15, 15, 0, "IN", 1 },
                    { 5, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 5, 20, 20, 0, "IN", 1 },
                    { 6, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 6, 10, 10, 0, "IN", 1 },
                    { 7, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 7, 25, 25, 0, "IN", 1 },
                    { 8, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 8, 100, 100, 0, "IN", 1 },
                    { 9, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 9, 300, 300, 0, "IN", 1 },
                    { 10, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 10, 20, 20, 0, "IN", 1 },
                    { 11, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 11, 60, 60, 0, "IN", 1 },
                    { 12, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 12, 15, 15, 0, "IN", 1 },
                    { 13, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 13, 10, 10, 0, "IN", 1 },
                    { 14, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 14, 8, 8, 0, "IN", 1 },
                    { 15, new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), "Initial stock received", 15, 30, 30, 0, "IN", 1 },
                    { 16, new DateTime(2024, 2, 4, 9, 0, 0, 0, DateTimeKind.Utc), "Sold to Dept. A", 1, 3, 27, 30, "OUT", 2 },
                    { 17, new DateTime(2024, 2, 4, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1022", 5, 8, 12, 20, "OUT", 2 },
                    { 18, new DateTime(2024, 2, 5, 9, 0, 0, 0, DateTimeKind.Utc), "Office weekly supply", 9, 50, 250, 300, "OUT", 3 },
                    { 19, new DateTime(2024, 2, 6, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1023", 7, 4, 21, 25, "OUT", 2 },
                    { 20, new DateTime(2024, 2, 7, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1024", 3, 3, 17, 20, "OUT", 2 },
                    { 21, new DateTime(2024, 2, 8, 9, 0, 0, 0, DateTimeKind.Utc), "New office setup", 12, 2, 13, 15, "OUT", 2 },
                    { 22, new DateTime(2024, 2, 9, 9, 0, 0, 0, DateTimeKind.Utc), "CEO office upgrade", 13, 1, 9, 10, "OUT", 2 },
                    { 23, new DateTime(2024, 2, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1025", 15, 5, 25, 30, "OUT", 3 },
                    { 24, new DateTime(2024, 2, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Sold to IT dept", 2, 3, 12, 15, "OUT", 2 },
                    { 25, new DateTime(2024, 2, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Training room supply", 11, 10, 50, 60, "OUT", 3 },
                    { 26, new DateTime(2024, 3, 2, 9, 0, 0, 0, DateTimeKind.Utc), "Restock from TechWorld", 5, 10, 22, 12, "IN", 1 },
                    { 27, new DateTime(2024, 3, 3, 9, 0, 0, 0, DateTimeKind.Utc), "Monthly restock", 9, 100, 350, 250, "IN", 1 },
                    { 28, new DateTime(2024, 3, 4, 9, 0, 0, 0, DateTimeKind.Utc), "Restock order", 4, 10, 25, 15, "IN", 1 },
                    { 29, new DateTime(2024, 3, 4, 9, 0, 0, 0, DateTimeKind.Utc), "Bulk cable purchase", 8, 50, 150, 100, "IN", 1 },
                    { 30, new DateTime(2024, 3, 7, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1030", 1, 2, 25, 27, "OUT", 2 },
                    { 31, new DateTime(2024, 3, 8, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1031", 3, 3, 14, 17, "OUT", 2 },
                    { 32, new DateTime(2024, 3, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1032", 5, 12, 10, 22, "OUT", 2 },
                    { 33, new DateTime(2024, 3, 10, 9, 0, 0, 0, DateTimeKind.Utc), "3rd floor office fit-out", 12, 3, 10, 13, "OUT", 2 },
                    { 34, new DateTime(2024, 3, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Monthly office supply", 9, 100, 250, 350, "OUT", 3 },
                    { 35, new DateTime(2024, 3, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Network upgrade project", 6, 2, 8, 10, "OUT", 2 },
                    { 36, new DateTime(2024, 3, 12, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1035", 7, 3, 18, 21, "OUT", 2 },
                    { 37, new DateTime(2024, 3, 13, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1036", 2, 4, 8, 12, "OUT", 2 },
                    { 38, new DateTime(2024, 3, 14, 9, 0, 0, 0, DateTimeKind.Utc), "Stationery replenishment", 10, 8, 12, 20, "OUT", 3 },
                    { 39, new DateTime(2024, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1037", 15, 8, 17, 25, "OUT", 2 },
                    { 40, new DateTime(2024, 4, 6, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1040 - cleared stock", 5, 10, 0, 10, "OUT", 2 },
                    { 41, new DateTime(2024, 4, 7, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1041", 4, 22, 3, 25, "OUT", 2 },
                    { 42, new DateTime(2024, 4, 8, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1042", 10, 8, 4, 12, "OUT", 3 },
                    { 43, new DateTime(2024, 4, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Sales order #1043", 14, 6, 2, 8, "OUT", 2 },
                    { 44, new DateTime(2024, 4, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Network expansion", 6, 2, 6, 8, "OUT", 2 },
                    { 45, new DateTime(2024, 4, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Cabling project bulk", 8, 70, 80, 150, "OUT", 3 },
                    { 46, new DateTime(2024, 4, 12, 9, 0, 0, 0, DateTimeKind.Utc), "Monthly supply", 9, 50, 200, 250, "OUT", 3 },
                    { 47, new DateTime(2024, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), "Training supply", 11, 5, 45, 50, "OUT", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_ProductId",
                table: "Alerts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProductId",
                table: "StockTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_UserId",
                table: "StockTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
