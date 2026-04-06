# BizI - PilotSale Management System

BizI is a robust, multi-tenant Sales and Inventory Management System built with **.NET 8** and **Clean Architecture**. It is designed to handle multiple independent businesses (tenants) within a single deployed instance, ensuring data isolation and providing essential retail tools like product management, order processing, and inventory control.

## 🏛️ Architecture: Clean Architecture

The project follows the principles of Clean Architecture to ensure separation of concerns, maintainability, and testability.

- **`src.BizI.Domain`**: The core of the system. Contains entities (Product, Order, etc.), enums, and repository interfaces. No external dependencies.
- **`src.BizI.Application`**: Business logic layer. Implements **CQRS** using **MediatR**. Contains Commands, Queries, Handlers, Validators (FluentValidation), and Service interfaces.
- **`src.BizI.Infrastructure`**: Implementation of external concerns. Handles data access via **LiteDB**, authentication logic, and multi-tenant provider logic.
- **`src.BizI.Api`**: The entry point. ASP.NET Core Web API with Controllers, Middleware (for error handling and tenant identification), and dependency injection configuration.

## 🛠️ Tech Stack

- **Framework**: .NET 8 (C#)
- **Database**: [LiteDB](https://www.litedb.org/) (Embedded NoSQL database)
- **Patterns**: CQRS with [MediatR](https://github.com/jbogard/MediatR), Repository Pattern
- **Authentication**: JWT (JSON Web Token) + BCrypt for password hashing
- **Validation**: [FluentValidation](https://fluentvalidation.net/)
- **Logging**: [Serilog](https://serilog.net/)
- **Documentation**: Swagger/OpenAPI

## 🚀 How to Run Project

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Steps

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd PilotSale
   ```
2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
3. **Run the API**:
   ```bash
   cd src/BizI.Api
   dotnet run
   ```
4. **Access Swagger UI**:
   Open your browser and navigate to: `http://localhost:<port>/swagger` (check console for port number).

## 📂 Folder Structure Explanation

- `src/BizI.Domain/`: `Entities`, `Enums`, `Interfaces` (Core abstractions)
- `src/BizI.Application/`: `Commands`, `Queries`, `Handlers`, `Services`, `Validators` (Business operations)
- `src/BizI.Infrastructure/`: `Data` (LiteDB implementation), `Auth`, `Services` (External dependencies)
- `src/BizI.Api/`: `Controllers`, `Middleware`, `Program.cs` (Host)

## 🌐 API Overview

### 🔐 Authentication

- `POST /api/auth/register`: Create a new account.
- `POST /api/auth/login`: Authenticate and receive a JWT token.

### 📦 Products

- `GET /api/product`: List all products.
- `POST /api/product`: Add a new product.
- `PUT /api/product/{id}`: Update product details.

### 🧾 Orders

- `POST /api/order`: Create a new sale order.
- `POST /api/order/return/{id}`: Process an order return.

### 📉 Inventory

- `POST /api/inventory/import`: Manually import/add stock.
- `POST /api/inventory/export`: Manually export/remove stock.
- `GET /api/inventory/log`: (Coming Soon) View stock movement history.

## 🏢 Multi-tenant Explanation

BizI uses a **Database-per-Tenant** isolation strategy implemented at a lightweight scale using **LiteDB**.

- When a user logs in, their `TenantId` is embedded in their JWT claim.
- The `ITenantProvider` extracts this `TenantId` from the request.
- The `LiteDbRepository` dynamically opens a database file named `tenant_{tenantId}.db` located in the `App_Data/` (or root) folder.
- This ensures that data from Store A never mixes with data from Store B.

## 🔄 Inventory Flow

### 📥 Import (Manual)

When you receive new stock from suppliers, use the `Inventory/Import` endpoint.

- **Action**: Increases `StockLevel` of the product.
- **Log**: Creates an `InventoryLog` entry (if implemented).

### 📤 Export (Manual)

Used for manual adjustments, breakage, or internal usage.

- **Action**: Decreases `StockLevel` of the product.

### 🛒 Sales Export (Automatic)

Triggered automatically when a **CreateOrder** command is processed.

- **Action**: Decreases `StockLevel` for each item in the order.
- **Security Check**: The order fails if any item has insufficient stock.

### 🔙 Order Return (Automatic)

Triggered via `Order/Return`.

- **Action**: Increases `StockLevel` back for the returned items.
- **Audit**: Links the stock increase back to the original order ID.

---

## 🗃️ Database Migrations (EF Core CLI — Required)

> ⚠️ **The database is NOT created automatically at startup.**
> All schema changes MUST go through EF Core CLI migrations.

### Prerequisites

Install the EF Core CLI tool (once per machine):

```bash
dotnet tool install --global dotnet-ef
```

---

### 1. Create a new migration

```bash
dotnet ef migrations add <MigrationName> \
  --project src/BizI.Infrastructure \
  --startup-project src/BizI.Api
```

Example (initial setup):

```bash
dotnet ef migrations add Init \
  --project src/BizI.Infrastructure \
  --startup-project src/BizI.Api
```

---

### 2. Apply migrations / update the database

```bash
dotnet ef database update \
  --project src/BizI.Infrastructure \
  --startup-project src/BizI.Api
```

---

### 3. Remove the last unapplied migration

```bash
dotnet ef migrations remove \
  --project src/BizI.Infrastructure \
  --startup-project src/BizI.Api
```

```bash
# 1. Apply schema
dotnet ef database update --project src/BizI.Infrastructure --startup-project src/BizI.Api

# 2. Seed data (idempotent — safe to run multiple times)
dotnet run --project src/BizI.Api -- --seed
```

---

### 4. List all migrations

```bash
dotnet ef migrations list \
  --project src/BizI.Infrastructure \
  --startup-project src/BizI.Api
```

---

### ⚠️ Safety Rules

- **Never** call `EnsureCreated()` or `Database.Migrate()` in application code.
- **Never** auto-apply migrations on startup.
- All schema changes go through migration files tracked in source control.

---

## 🔮 Future-Proof Database Switch

Only one line in [`InfrastructureServiceExtensions.cs`](src/BizI.Infrastructure/DependencyInjection/InfrastructureServiceExtensions.cs) ever needs to change:

```csharp
// SQLite (current)
options.UseSqlite(connectionString)

// → SQL Server
options.UseSqlServer(connectionString)

// → PostgreSQL
options.UseNpgsql(connectionString)
```

No other layer (Domain, Application, API) changes.

---

Developed with ❤️ by the PilotSale Team.
