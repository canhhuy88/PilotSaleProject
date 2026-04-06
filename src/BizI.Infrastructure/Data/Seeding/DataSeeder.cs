using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var user = new User
            {
                Id = "admin",
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"), // Needs proper hash in real system
                FullName = "System Admin",
                RoleId = "Admin",
                IsActive = true
            };

            await _userRepo.AddAsync(user);
        }
    }

    private async Task SeedCustomersAsync()
    {
        var walkin = await _customerRepo.GetByIdAsync("WALKIN");
        if (walkin == null)
        {
            var customer = new Customer
            {
                Id = "WALKIN",
                Name = "Walk-in Customer",
                CustomerType = CustomerType.WALKIN,
                Phone = ""
            };

            await _customerRepo.AddAsync(customer);
        }
    }

    private async Task SeedProductsAsync()
    {
        var products = await _productRepo.GetAllAsync();
        if (!products.Any())
        {
            await _productRepo.AddAsync(new Product { Id = Guid.NewGuid().ToString("N"), Name = "Sample Product 1", SKU = "SP001", SalePrice = 100000, CostPrice = 80000 });
            await _productRepo.AddAsync(new Product { Id = Guid.NewGuid().ToString("N"), Name = "Sample Product 2", SKU = "SP002", SalePrice = 200000, CostPrice = 150000 });
        }
    }
}
