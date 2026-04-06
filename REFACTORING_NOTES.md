# 📋 BizI — Clean Architecture Refactoring Notes

> Thực hiện: 2026-04-06  
> Mục tiêu: Tách hoàn toàn tầng database ra khỏi business logic, áp dụng DDD, SOLID, Clean Architecture  
> Sau này chỉ cần đọc file này là nhớ lại toàn bộ quyết định kiến trúc

---

## 🏗️ Cấu Trúc Dự Án Sau Refactor

```
src/
 ├── BizI.Domain/                    ← Không phụ thuộc vào bất kỳ thư viện nào
 │    ├── Entities/
 │    │    ├── BaseEntity.cs         ← Thêm MarkAsDeleted(), Touch()
 │    │    ├── Product.cs            ← Aggregate Root, có Create(), UpdateDetails()
 │    │    ├── Category.cs           ← Create(), Rename()
 │    │    ├── Customer.cs           ← EarnLoyaltyPoints(), AddDebt(), RecordOrder()
 │    │    ├── Order.cs              ← Aggregate Root: Complete(), Cancel(), MarkAsReturned()
 │    │    ├── OrderItem.cs          ← Create(), Return(qty), LineTotal, RemainingQuantity
 │    │    ├── Inventory.cs          ← AddStock(), DeductStock(), SetQuantity()
 │    │    └── User.cs               ← Create(), ChangePassword(), Activate/Deactivate()
 │    ├── Enums/
 │    │    ├── OrderStatus.cs        ← Pending, Completed, Cancelled, Returned
 │    │    ├── CustomerType.cs       ← WalkIn, Regular, Vip, Wholesale (chuyển từ Customer.cs)
 │    │    ├── PaymentStatus.cs      ← Unpaid, PartiallyPaid, Paid, Refunded (mới)
 │    │    ├── InventoryTransactionType.cs
 │    │    └── UserRole.cs
 │    ├── Interfaces/
 │    │    ├── IRepository.cs        ← Generic CRUD interface
 │    │    ├── IInventoryRepository.cs  ← Mới: GetByProductAndWarehouseAsync, GetByWarehouseAsync
 │    │    ├── IOrderRepository.cs      ← Mới: GetByCodeAsync, GetByCustomerAsync, GetByStatusAsync
 │    │    ├── IProductRepository.cs    ← Mới: GetBySkuAsync, GetByCategoryAsync, GetActiveProductsAsync
 │    │    └── ITenantProvider.cs
 │    └── Exceptions/
 │         ├── DomainException.cs
 │         ├── InsufficientStockException.cs  ← Thêm overload (productId, available, requested)
 │         └── OverReturnException.cs
 │
 ├── BizI.Application/               ← Use cases, không access DB trực tiếp
 │    ├── Common/
 │    │    ├── CommandResult.cs      ← Thêm Id property, SuccessResult(string id) overload
 │    │    └── ValidationBehavior.cs ← Cleaned up, XML docs
 │    ├── Features/
 │    │    ├── Products/             ← Dùng IProductRepository + Product.Create()
 │    │    │    ├── CreateProduct.cs ← Check duplicate SKU, dùng Product.Create()
 │    │    │    ├── GetAllProducts.cs← Trả về ProductDto (có GrossMarginPercent)
 │    │    │    ├── GetProductById.cs
 │    │    │    ├── UpdateProduct.cs ← Dùng product.UpdateDetails()
 │    │    │    └── DeleteProduct.cs
 │    │    └── Orders/
 │    │         └── CreateOrder.cs  ← Dùng Order.Create(), OrderItem.Create(), order.Complete()
 │    ├── Interfaces/
 │    │    ├── IInventoryService.cs
 │    │    └── IAuthService.cs
 │    └── Services/
 │         └── InventoryService.cs  ← Dùng IInventoryRepository + domain methods (AddStock, DeductStock)
 │
 ├── BizI.Infrastructure/            ← Toàn bộ code liên quan DB ở đây
 │    ├── Persistence/
 │    │    ├── LiteDb/
 │    │    │    ├── ILiteDbContext.cs        ← Interface abstraction (ĐIỂM SWAP DATABASE)
 │    │    │    ├── LiteDbContext.cs         ← Implements ILiteDbContext, config indices
 │    │    │    └── CollectionNames.cs       ← Hằng số tên collection, không magic string
 │    │    └── Repositories/
 │    │         ├── LiteDbRepository.cs         ← Generic, implements IRepository<T>
 │    │         ├── LiteDbInventoryRepository.cs ← Implements IInventoryRepository
 │    │         ├── LiteDbOrderRepository.cs     ← Implements IOrderRepository
 │    │         └── LiteDbProductRepository.cs   ← Implements IProductRepository
 │    ├── Auth/
 │    │    └── AuthService.cs
 │    ├── Services/
 │    │    └── TenantProvider.cs
 │    ├── Data/                      ← Giữ nguyên: Migrations, Seeding
 │    │    ├── Migrations/
 │    │    └── Seeding/
 │    └── DependencyInjection/
 │         └── InfrastructureServiceExtensions.cs  ← AddInfrastructure() extension method
 │
 └── BizI.Api/                       ← Controllers mỏng (thin), không có business logic
      ├── Endpoints/
      │    ├── ProductEndpoints.cs   ← Static extension method MapProductEndpoints()
      │    ├── InventoryEndpoints.cs ← Static extension method MapInventoryEndpoints()
      │    ├── OrderEndpoints.cs     ← Static extension method MapOrderEndpoints()
      │    └── AuthEndpoints.cs
      ├── Middleware/
      │    ├── CorrelationIdMiddleware.cs
      │    ├── GlobalExceptionMiddleware.cs
      │    └── Middlewares.cs  (TenantMiddleware)
      └── Program.cs                 ← Chỉ có 1 dòng: services.AddInfrastructure(configuration)
```

---

## 🔑 Nguyên Tắc Áp Dụng Và Ví Dụ

### 1. Domain Entities Không Còn Anemic (DDD)

**TRƯỚC:**

```csharp
// Entity chỉ là data bag
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
}

// Handler phải tự set thủ công
var product = new Product { Name = request.Name, SalePrice = request.Price };
```

**SAU:**

```csharp
// Entity có business logic
public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal SalePrice { get; private set; }
    public decimal GrossMarginPercent => SalePrice == 0 ? 0 : Math.Round((SalePrice - CostPrice) / SalePrice * 100, 2);

    public static Product Create(string name, string sku, decimal costPrice, decimal salePrice, ...)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("...");
        if (salePrice < 0) throw new DomainException("...");
        return new Product { Name = name, ... };
    }

    public void UpdateDetails(string name, ...) { /* tự validate rồi set */ Touch(); }
    public void Activate() => IsActive = true;
}
```

### 2. Repository Interface Ở Domain, Implement Ở Infrastructure (SOLID - DIP)

**Domain** định nghĩa:

```csharp
// src/BizI.Domain/Interfaces/IInventoryRepository.cs
public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId);
    Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId);
}
```

**Infrastructure** implement:

```csharp
// src/BizI.Infrastructure/Persistence/Repositories/LiteDbInventoryRepository.cs
public class LiteDbInventoryRepository : LiteDbRepository<Inventory>, IInventoryRepository
{
    public Task<Inventory?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId)
    {
        var result = Collection.FindOne(x => x.ProductId == productId && x.WarehouseId == warehouseId && !x.IsDeleted);
        return Task.FromResult<Inventory?>(result);
    }
}
```

### 3. Database Abstraction — Điểm Swap Database

```csharp
// src/BizI.Infrastructure/Persistence/LiteDb/ILiteDbContext.cs
public interface ILiteDbContext : IDisposable
{
    ILiteCollection<T> GetCollection<T>(string name);
}

// LiteDB implementation
public sealed class LiteDbContext : ILiteDbContext { ... }

// Tương lai: MySQL implementation
// public sealed class MySqlDbContext : IDbContext { ... }
```

### 4. DI Extension — Đổi Database Chỉ Cần Thay Đây

```csharp
// src/BizI.Infrastructure/DependencyInjection/InfrastructureServiceExtensions.cs
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    // 👇 ĐỔI DATABASE: chỉ thay 3 dòng này
    services.AddSingleton<ILiteDbContext>(sp => new LiteDbContext(connectionString, logger));
    services.AddScoped<IProductRepository, LiteDbProductRepository>();
    services.AddScoped<IInventoryRepository, LiteDbInventoryRepository>();
    services.AddScoped<IOrderRepository, LiteDbOrderRepository>();
    // ☝️ Đổi thành MySqlProductRepository, EfCoreInventoryRepository, v.v.

    // Các service khác không đổi
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IInventoryService, InventoryService>();
    return services;
}
```

### 5. Program.cs — Không Còn Reference LiteDB

**TRƯỚC:**

```csharp
using LiteDB;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "biz_i.db";
var appDbContext = new AppDbContext(connectionString);
builder.Services.AddSingleton(appDbContext);
builder.Services.AddSingleton<ILiteDatabase>(appDbContext.Database);
builder.Services.AddScoped(typeof(IRepository<>), typeof(LiteDbRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
// ... 20 dòng DI thủ công
```

**SAU:**

```csharp
// Không có using LiteDB nào
builder.Services.AddInfrastructure(builder.Configuration); // 1 dòng duy nhất
```

### 6. CommandResult — Thêm Id Property

```csharp
public class CommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Id { get; set; }        // ← MỚI: Id của entity vừa tạo/sửa
    public object? Data { get; set; }

    public static CommandResult SuccessResult(object? data = null) => new() { Success = true, Data = data };
    public static CommandResult SuccessResult(string id, object? data = null) => new() { Success = true, Id = id, Data = data };
    public static CommandResult FailureResult(string message) => new() { Success = false, Message = message };
}
```

---

## 🔄 Hướng Dẫn Đổi Database Về Sau

### Từ LiteDB → MySQL/PostgreSQL/SQL Server

**Bước 1:** Tạo DbContext mới trong `Infrastructure/Persistence/MySql/`

```csharp
public class MySqlDbContext : DbContext, IMyDbContext
{
    public DbSet<Product> Products { get; set; }
    // ...
}
```

**Bước 2:** Tạo repository mới cho từng aggregate

```csharp
public class MySqlProductRepository : IProductRepository
{
    private readonly MySqlDbContext _context;
    public Task<Product?> GetBySkuAsync(string sku) => _context.Products.FirstOrDefaultAsync(x => x.SKU == sku);
    // ...
}
```

**Bước 3:** Trong `AddInfrastructure()`, swap DI:

```csharp
// XÓA dòng LiteDB:
// services.AddSingleton<ILiteDbContext>(...);
// services.AddScoped<IProductRepository, LiteDbProductRepository>();

// THÊM dòng MySQL:
services.AddDbContext<MySqlDbContext>(o => o.UseMySQL(connectionString));
services.AddScoped<IProductRepository, MySqlProductRepository>();
services.AddScoped<IInventoryRepository, MySqlInventoryRepository>();
services.AddScoped<IOrderRepository, MySqlOrderRepository>();
```

**Kết quả:** Domain, Application, API → **KHÔNG CẦN THAY ĐỔI GÌ CẢ**

---

## 📦 Các File Quan Trọng Cần Nhớ

| File                                                                                                                   | Vai Trò                                    |
| ---------------------------------------------------------------------------------------------------------------------- | ------------------------------------------ |
| [`ILiteDbContext.cs`](src/BizI.Infrastructure/Persistence/LiteDb/ILiteDbContext.cs)                                    | Điểm abstraction — swap database ở đây     |
| [`CollectionNames.cs`](src/BizI.Infrastructure/Persistence/LiteDb/CollectionNames.cs)                                  | Hằng số tên collection, không magic string |
| [`InfrastructureServiceExtensions.cs`](src/BizI.Infrastructure/DependencyInjection/InfrastructureServiceExtensions.cs) | `AddInfrastructure()` — toàn bộ DI wiring  |
| [`IProductRepository.cs`](src/BizI.Domain/Interfaces/IProductRepository.cs)                                            | Interface domain-specific cho Product      |
| [`IInventoryRepository.cs`](src/BizI.Domain/Interfaces/IInventoryRepository.cs)                                        | Interface domain-specific cho Inventory    |
| [`IOrderRepository.cs`](src/BizI.Domain/Interfaces/IOrderRepository.cs)                                                | Interface domain-specific cho Order        |
| [`BaseEntity.cs`](src/BizI.Domain/Entities/BaseEntity.cs)                                                              | `MarkAsDeleted()` + `Touch()`              |
| [`CommandResult.cs`](src/BizI.Application/Common/CommandResult.cs)                                                     | Kết quả command, có `Id` property          |

---

## ⚠️ Những Thứ Chưa Làm / TODO Tương Lai

- [ ] AutoMapper: file `AutoMapperProfile.cs` đã tạo nhưng chưa dùng — cân nhắc dùng cho DTO mapping
- [ ] Unit tests: cần cập nhật các test trong `tests/` để mock `IProductRepository` thay vì `IRepository<Product>`
- [ ] Các entity khác (`Supplier`, `Warehouse`, `CustomerGroup`...) chưa được enrich — áp dụng cùng pattern
- [ ] File cũ `AppDbContext.cs` và `LiteDbRepository.cs` ở `Data/` vẫn còn — xóa đi khi build ổn định
- [ ] `Class1.cs` placeholder ở mỗi project — xóa đi
- [ ] Thêm `ICustomerRepository`, `IUserRepository` nếu cần query domain-specific

---

## 🧠 Nguyên Tắc Quan Trọng Cần Nhớ

1. **Domain không import gì từ Infrastructure** — Domain chỉ throw `DomainException`, không biết LiteDB tồn tại
2. **Application không access DB trực tiếp** — chỉ dùng interface từ Domain
3. **Infrastructure là nơi duy nhất biết về LiteDB** — `using LiteDB` chỉ xuất hiện ở Infrastructure
4. **API layer mỏng tuyệt đối** — endpoint chỉ gọi `mediator.Send()` rồi map result → HTTP response
5. **Không có magic string** — dùng `CollectionNames.Products` không dùng `"Products"`
6. **Soft delete** — không xóa thật, chỉ set `IsDeleted = true` qua `MarkAsDeleted()`
7. **Factory methods bắt buộc** — không dùng `new Product { }` trực tiếp, luôn dùng `Product.Create(...)`
