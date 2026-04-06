using AutoMapper;
using BizI.Domain.Entities;

namespace BizI.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Category
        CreateMap<Category, Category>(); // Placeholder for future mapping

        // Customer
        CreateMap<Customer, Customer>();
        CreateMap<CustomerGroup, CustomerGroup>();

        // ImportOrder
        CreateMap<ImportOrder, ImportOrder>();

        // Inventory
        CreateMap<BizI.Domain.Entities.Inventory, BizI.Domain.Entities.Inventory>();
        CreateMap<InventoryTransaction, InventoryTransaction>();

        // Order
        CreateMap<Order, Order>();
        CreateMap<Payment, Payment>();
        CreateMap<Debt, Debt>();
        CreateMap<ReturnOrder, ReturnOrder>();

        // Product
        CreateMap<Product, Product>();
        CreateMap<ProductVariant, ProductVariant>();

        // Role
        CreateMap<Role, Role>();
        CreateMap<User, User>();

        // Stock
        CreateMap<StockItem, StockItem>();
        CreateMap<StockIn, StockIn>();
        CreateMap<StockOut, StockOut>();
        CreateMap<StockTransfer, StockTransfer>();
        CreateMap<StockAudit, StockAudit>();

        // Supplier
        CreateMap<Supplier, Supplier>();

        // Warehouse
        CreateMap<Warehouse, Warehouse>();

        // AuditLog
        CreateMap<AuditLog, AuditLog>();
    }
}
