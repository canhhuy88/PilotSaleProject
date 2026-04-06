using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using BizI.Application.Common;
using BizI.Application.Interfaces;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;

namespace BizI.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepo;
    private readonly IConfiguration _configuration;

    public AuthService(IRepository<User> userRepo, IConfiguration configuration)
    {
        _userRepo = userRepo;
        _configuration = configuration;
    }

    public async Task<CommandResult> LoginAsync(string username, string password)
    {
        var alluser = await _userRepo.GetAllAsync();
        var user = (await _userRepo.FindAsync(u => u.Username == username)).FirstOrDefault();
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return CommandResult.FailureResult("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        return CommandResult.SuccessResult(new { Token = token, Username = user.Username, Role = user.RoleId });
    }

    public async Task<CommandResult> RegisterAsync(string username, string password, UserRole role)
    {
        var existing = (await _userRepo.FindAsync(u => u.Username == username)).FirstOrDefault();
        if (existing != null)
        {
            return CommandResult.FailureResult("Username already exists");
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = role.ToString()
        };

        await _userRepo.AddAsync(user);
        return CommandResult.SuccessResult();
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "super_secret_key_1234567890123456");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.RoleId)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
