using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;

namespace BizI.Infrastructure.Data.Seeding;

public class DataSeeder : IDataSeeder
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly IRepository<Product> _productRepo;

    public DataSeeder(
        IRepository<User> userRepo,
        IRepository<Customer> customerRepo,
        IRepository<Product> productRepo)
    {
        _userRepo = userRepo;
        _customerRepo = customerRepo;
        _productRepo = productRepo;
    }

    public async Task SeedAsync()
    {
        await SeedUsersAsync();
        await SeedCustomersAsync();
        await SeedProductsAsync();
    }

    private async Task SeedUsersAsync()
    {
        var users = await _userRepo.GetAllAsync();
        if (!users.Any())
        {
            // ✅ Use Domain factory — User has private setters
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            var user = User.Create(
                username: "admin",
                passwordHash: passwordHash,
                fullName: "System Admin",
                roleId: "Admin");

            await _userRepo.AddAsync(user);
        }
    }

    private async Task SeedCustomersAsync()
    {
        var walkin = await _customerRepo.GetByIdAsync("WALKIN");
        if (walkin is null)
        {
            // ✅ Use domain factory CreateWalkIn() — respects private setters
            var customer = Customer.CreateWalkIn();
            await _customerRepo.AddAsync(customer);
        }
    }

    private async Task SeedProductsAsync()
    {
        var products = await _productRepo.GetAllAsync();
        if (!products.Any())
        {
            // ✅ Use Domain factory — Product has private setters; prices are Money VOs
            var p1 = Product.Create(
                name: "Sample Product 1",
                sku: "SP001",
                costPrice: 80_000m,
                salePrice: 100_000m,
                unit: "pcs",
                categoryId: "default");

            var p2 = Product.Create(
                name: "Sample Product 2",
                sku: "SP002",
                costPrice: 150_000m,
                salePrice: 200_000m,
                unit: "pcs",
                categoryId: "default");

            await _productRepo.AddAsync(p1);
            await _productRepo.AddAsync(p2);
        }
    }
}
