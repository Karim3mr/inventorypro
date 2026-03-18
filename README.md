# 📦 InventoryPro — Full-Stack Inventory Management System

A production-ready inventory management system built with **ASP.NET Core 8 Web API** and **React 18**, featuring JWT authentication, role-based access control, real-time stock tracking, and analytics dashboards.

---

## 🚀 Features

| Feature | Description |
|---|---|
| 🔐 JWT Auth + Roles | Admin / Manager / Staff role-based access |
| 📦 Product Management | Full CRUD with SKU, barcode, pricing, and category |
| 🏷️ Category Management | Organize products into custom categories |
| 🚚 Supplier Management | Track supplier contacts and product relationships |
| 📊 Stock Transactions | IN / OUT / ADJUSTMENT with full audit trail |
| 🔔 Smart Alerts | Auto-generated low stock & out-of-stock alerts |
| 📈 Reports & Dashboard | Bar charts, pie charts, and movement summaries |
| 🔲 Barcode Support | Generate and display barcodes per product |

---

## 💡 Key Highlights

- Role-based authentication system (Admin / Manager / Staff)
- Real-time inventory tracking with stock updates
- Automated low-stock and out-of-stock alert system
- Full audit trail for all stock transactions
- Interactive analytics dashboard with charts and reports

---

## 🛠️ Tech Stack

**Backend**
- ASP.NET Core 8 Web API
- Entity Framework Core 8 (SQL Server)
- JWT Bearer Authentication
- BCrypt.Net password hashing
- Swagger / OpenAPI

**Frontend**
- React 18 + Vite
- React Router v6
- Axios (with interceptors)
- Recharts (dashboard charts)
- Tailwind CSS
- react-barcode
- react-hot-toast

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- SQL Server (local or Docker)

---

### 1. Clone the Repository

```bash
git clone https://github.com/Karim3mr/inventorypro.git
cd inventorypro
```

---

### 2. Backend Setup

```bash
cd backend

# Restore packages
dotnet restore

# Update connection string in appsettings.json
# "Default": "Server=localhost;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True"

# Run migrations & seed data (auto-runs on startup)
dotnet run
```

The API will be available at `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

---

### 3. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Start dev server
npm run dev
```

The app will be available at `http://localhost:5173`

---

## 🔑 Default Login

| Email | Password | Role |
|---|---|---|
| admin@inventory.com | Admin@123 | Admin |

> Admins can create Manager and Staff accounts from within the app.

---

## 📁 Project Structure

```
InventoryPro/
├── backend/
│   ├── Controllers/        # API endpoints
│   ├── Data/               # EF Core DbContext & migrations
│   ├── DTOs/               # Request/Response models
│   ├── Models/             # Domain entities
│   ├── Services/           # JwtService, AlertService
│   ├── appsettings.json
│   └── Program.cs
│
└── frontend/
    └── src/
        ├── components/     # Layout, shared UI
        ├── context/        # AuthContext (JWT state)
        ├── pages/          # Dashboard, Products, Stock, etc.
        └── services/       # Axios API calls
```

---

## 🔒 Role Permissions

| Action | Admin | Manager | Staff |
|---|---|---|---|
| View all data | ✅ | ✅ | ✅ |
| Create/Edit products | ✅ | ✅ | ❌ |
| Stock transactions | ✅ | ✅ | ✅ |
| Resolve alerts | ✅ | ✅ | ❌ |
| Delete / deactivate | ✅ | ❌ | ❌ |
| Register new users | ✅ | ❌ | ❌ |

---

## 📸 Screenshots

### Dashboard Overview
![Dashboard](docs/screenshots/dashboard.png)

### Dashboard Charts
![Dashboard Charts](docs/screenshots/dashboard-charts.png)

### Products Management
![Products](docs/screenshots/products.png)

### Reports - Top Products
![Reports Top](docs/screenshots/reports-top-products.png)

### Reports - Stock Movement
![Reports Stock](docs/screenshots/reports-stock.png)

### Stock Transactions
![Stock](docs/screenshots/stock.png)

### Alerts System
![Alerts](docs/screenshots/alerts.png)

---

## 🧠 System Overview

InventoryPro is designed to help businesses manage their inventory efficiently.

It allows users to:
- Track product quantities in real-time
- Monitor stock movement (IN / OUT)
- Receive alerts for low or out-of-stock items
- Analyze inventory data through charts and reports

The system supports multiple user roles with different permissions to ensure secure and organized operations.

---

## 📄 License

MIT — free to use and modify.
