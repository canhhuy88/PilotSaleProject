//using BizI.Domain.Entities;

//namespace BizI.Domain.Interfaces;

///// <summary>
///// Specialized repository for the Product aggregate.
///// </summary>
//public interface IProductRepository : IRepository<Product>
//{
//    /// <summary>Returns a product by its SKU, or null if not found.</summary>
//    Task<Product?> GetBySkuAsync(string sku);

//    /// <summary>Returns all products belonging to a given category.</summary>
//    Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);

//    /// <summary>Returns all active products.</summary>
//    Task<IEnumerable<Product>> GetActiveProductsAsync();
//}
