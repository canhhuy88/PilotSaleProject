using AutoMapper;
using BizI.Application.DTOs.ImportOrder;
using BizI.Application.DTOs.Inventory;
using BizI.Application.Features.AuditLogs.Dtos;
using BizI.Application.Features.Categories.Dtos;
using BizI.Application.Features.CustomerGroups.Dtos;
using BizI.Application.Features.Customers.Dtos;
using BizI.Application.Features.Orders.Dtos;
using BizI.Application.Features.PaymentAndReturns.Dtos;
using BizI.Application.Features.Products.Dtos;
using BizI.Application.Features.Roles.Dtos;
using BizI.Application.Features.Suppliers.Dtos;
using BizI.Application.Features.Users.Dtos;
using BizI.Application.Features.Warehouses.Dtos;
using BizI.Domain.Entities;
using BizI.Application.Features.StockTransactions.Dtos;
using BizI.Application.Features.StockItems.Dtos;
using BizI.Application.Features.StockOperations.Dtos;

namespace BizI.Application.Mappings;

/// <summary>
/// AutoMapper profile mapping Domain entities → Application DTOs.
/// Entities are NEVER exposed directly to the API layer.
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // ── Product ──────────────────────────────────────────────────────────
        // Product.CostPrice and SalePrice are Money value objects
        CreateMap<Product, ProductDto>()
            .ConstructUsing(p => new ProductDto(
                p.Id, p.Name, p.SKU, p.Description, p.Barcode,
                p.CategoryId,
                p.CostPrice.Amount,   // Money → decimal
                p.SalePrice.Amount,
                p.GrossMarginPercent,
                p.Unit, p.IsActive));

        // ── Category ─────────────────────────────────────────────────────────
        // Category has: Id, Name, ParentId, Description — NO IsActive
        CreateMap<Category, CategoryDto>()
            .ConstructUsing(c => new CategoryDto(c.Id, c.Name, c.ParentId, c.Description));

        // ── Customer ─────────────────────────────────────────────────────────
        // Customer.Phone is PhoneNumber VO; ContactAddress is Address VO
        CreateMap<Customer, CustomerDto>()
            .ConstructUsing(c => new CustomerDto(
                c.Id, c.Name,
                c.Phone != null ? c.Phone.Value : null,
                c.ContactAddress != null ? c.ContactAddress.FullAddress : null,
                c.CustomerType, c.LoyaltyPoints, c.LoyaltyTier,
                c.TotalSpent, c.TotalOrders, c.DebtTotal, c.DebtLimit,
                c.LastOrderDate));

        // ── CustomerGroup ─────────────────────────────────────────────────────
        CreateMap<CustomerGroup, CustomerGroupDto>()
            .ConstructUsing(g => new CustomerGroupDto(g.Id, g.Name, g.DiscountPercent));

        // ── Order ─────────────────────────────────────────────────────────────
        // Order amounts are Money VOs; Items is IReadOnlyCollection<OrderItem>
        CreateMap<Order, OrderDto>()
            .ConstructUsing(o => new OrderDto(
                o.Id, o.Code, o.CustomerId,
                o.TotalAmount.Amount,
                o.Discount.Amount,
                o.FinalAmount.Amount,
                o.TotalAmount.Currency,
                o.Status, o.PaymentStatus,
                o.CreatedBy,
                o.CreatedAt,
                o.Items.Select(i => new OrderItemDto(
                    i.ProductId, i.Quantity, i.ReturnedQuantity,
                    i.Price.Amount, i.LineTotal.Amount,
                    i.Price.Currency)).ToList()));

        // ── ImportOrder ───────────────────────────────────────────────────────
        CreateMap<ImportOrder, ImportOrderDto>()
            .ConstructUsing(io => new ImportOrderDto(
                io.Id, io.SupplierId,
                io.TotalAmount.Amount,
                io.TotalAmount.Currency,
                io.Status,
                io.CreatedAt,
                io.Items.Select(i => new ImportOrderItemDto(
                    i.ProductId, i.Quantity,
                    i.CostPrice.Amount,
                    i.CostPrice.Currency)).ToList()));

        // ── Inventory ────────────────────────────────────────────────────────
        CreateMap<Inventory, InventoryDto>()
            .ConstructUsing(inv => new InventoryDto(
                inv.Id, inv.ProductId, inv.WarehouseId, inv.Quantity));

        CreateMap<InventoryTransaction, InventoryTransactionDto>()
            .ConstructUsing(t => new InventoryTransactionDto(
                t.Id, t.ProductId, t.WarehouseId,
                t.Type, t.Quantity, t.ReferenceId, t.CreatedAt));

        // ── StockTransaction ─────────────────────────────────────────────────
        CreateMap<StockTransaction, StockTransactionDto>()
            .ConstructUsing(t => new StockTransactionDto(
                t.Id, t.ProductId, t.WarehouseId, t.Type.ToString(), t.Quantity, t.BeforeQty, t.AfterQty, t.RefId, t.CreatedBy, t.CreatedAt));

        // ── Payment ──────────────────────────────────────────────────────────
        CreateMap<Payment, PaymentDto>()
            .ConstructUsing(p => new PaymentDto(
                p.Id, p.OrderId,
                p.Amount.Amount,
                p.Amount.Currency,
                p.Method,
                p.CreatedAt));

        CreateMap<Debt, DebtDto>()
            .ConstructUsing(d => new DebtDto(
                d.Id, d.CustomerId, d.OrderId,
                d.Amount.Amount,
                d.PaidAmount.Amount,
                d.RemainingAmount.Amount,
                d.Status,
                d.Amount.Currency,
                d.CreatedAt));

        // ReturnOrder → ReturnOrderReadDto (disambiguated name)
        CreateMap<ReturnOrder, ReturnOrderReadDto>()
            .ConstructUsing(ro => new ReturnOrderReadDto(
                ro.Id, ro.OrderId,
                ro.TotalRefund.Amount,
                ro.TotalRefund.Currency,
                ro.CreatedAt,
                ro.Items.Select(i => new ReturnItemReadDto(
                    i.ProductId, i.Quantity,
                    i.RefundPrice.Amount,
                    i.RefundPrice.Currency)).ToList()));

        // ── Supplier ─────────────────────────────────────────────────────────
        // Supplier has: Name, Phone (PhoneNumber VO), ContactAddress (Address VO)
        CreateMap<Supplier, SupplierDto>()
            .ConstructUsing(s => new SupplierDto(
                s.Id, s.Name,
                s.Phone != null ? s.Phone.Value : null,
                s.ContactAddress != null ? s.ContactAddress.FullAddress : null));

        // ── Warehouse ────────────────────────────────────────────────────────
        CreateMap<Warehouse, WarehouseDto>()
            .ConstructUsing(w => new WarehouseDto(w.Id, w.Name, w.BranchId));

        // ── Role ─────────────────────────────────────────────────────────────
        CreateMap<Role, RoleDto>()
            .ConstructUsing(r => new RoleDto(r.Id, r.Name, r.Permissions.ToList()));

        // ── User ─────────────────────────────────────────────────────────────
        CreateMap<User, UserDto>()
            .ConstructUsing(u => new UserDto(u.Id, u.Username, u.FullName, u.RoleId, u.IsActive));
    }
}
