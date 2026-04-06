using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Seed.Seeders;

/// <summary>
/// Seeds default system users (admin, staff) with BCrypt-hashed passwords.
/// Idempotent — skips any user whose username already exists.
/// </summary>
public sealed class UserSeeder
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<Role> _roleRepo;

    public UserSeeder(IRepository<User> userRepo, IRepository<Role> roleRepo)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
    }

    public async Task SeedAsync()
    {
        var adminRole = (await _roleRepo.FindAsync(r => r.Name == "Admin")).FirstOrDefault();
        var staffRole = (await _roleRepo.FindAsync(r => r.Name == "Staff")).FirstOrDefault();

        if (adminRole is not null)
            await EnsureUserAsync("admin", "System Admin", adminRole.Id);

        if (staffRole is not null)
            await EnsureUserAsync("staff", "System Staff", staffRole.Id);
    }

    private async Task EnsureUserAsync(string username, string fullName, Guid roleId)
    {
        var existing = await _userRepo.FindAsync(u => u.Username == username);
        if (existing.Any()) return;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
        var user = User.Create(
            username: username,
            passwordHash: passwordHash,
            fullName: fullName,
            roleId: roleId);

        await _userRepo.AddAsync(user);
    }
}
