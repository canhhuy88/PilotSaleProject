using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Seed.Seeders;

/// <summary>
/// Seeds default system roles (Admin, Staff) if they do not already exist.
/// </summary>
public sealed class RoleSeeder
{
    private readonly IRepository<Role> _roleRepo;

    public RoleSeeder(IRepository<Role> roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task SeedAsync()
    {
        await EnsureRoleAsync("Admin");
        await EnsureRoleAsync("Staff");
    }

    private async Task EnsureRoleAsync(string name)
    {
        var existing = await _roleRepo.FindAsync(r => r.Name == name);
        if (!existing.Any())
        {
            var role = Role.Create(name);
            await _roleRepo.AddAsync(role);
        }
    }
}
