//using BizI.Domain.Entities;
//using BizI.Domain.Enums;

//namespace BizI.Domain.Interfaces;

///// <summary>
///// Specialized repository for the Order aggregate.
///// </summary>
//public interface IOrderRepository : IRepository<Order>
//{
//    /// <summary>Finds an order by its business code (e.g. "ORD-2024-001").</summary>
//    Task<Order?> GetByCodeAsync(string code);

//    /// <summary>Returns all orders for a specific customer.</summary>
//    Task<IEnumerable<Order>> GetByCustomerAsync(Guid customerId);

//    /// <summary>Returns all orders that match a given status.</summary>
//    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
//}
