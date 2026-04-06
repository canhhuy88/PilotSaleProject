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
    private readonly IRepository<RefreshToken> _refreshTokenRepo;
    private readonly IConfiguration _configuration;

    public AuthService(IRepository<User> userRepo, IRepository<RefreshToken> refreshTokenRepo, IConfiguration configuration)
    {
        _userRepo = userRepo;
        _refreshTokenRepo = refreshTokenRepo;
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

        var token = GenerateJwtToken(user, out var refreshTokenEntity);
        await _refreshTokenRepo.AddAsync(refreshTokenEntity);

        return CommandResult.SuccessResult(new { AccessToken = token, RefreshToken = refreshTokenEntity.Token, Username = user.Username, Role = user.RoleId });
    }

    public async Task<CommandResult> RegisterAsync(string username, string password, UserRole role)
    {
        var existing = (await _userRepo.FindAsync(u => u.Username == username)).FirstOrDefault();
        if (existing != null)
        {
            return CommandResult.FailureResult("Username already exists");
        }

        // ✅ Use Domain factory — User has private setters
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.Create(
            username,
            passwordHash,
            fullName: username,   // default full name = username for self-registration
            roleId: role.ToString());

        await _userRepo.AddAsync(user);
        return CommandResult.SuccessResult(user.Id);
    }

    public async Task<CommandResult> RefreshTokenAsync(string refreshToken)
    {
        var tokenEntity = (await _refreshTokenRepo.FindAsync(rt => rt.Token == refreshToken)).FirstOrDefault();

        if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiryDate <= DateTime.UtcNow)
        {
            return CommandResult.FailureResult("Invalid or expired refresh token");
        }

        var user = await _userRepo.GetByIdAsync(tokenEntity.UserId);
        if (user == null)
        {
            return CommandResult.FailureResult("User not found");
        }

        // Revoke the old token
        tokenEntity.Revoke();
        await _refreshTokenRepo.UpdateAsync(tokenEntity);

        // Generate new tokens
        var accessToken = GenerateJwtToken(user, out var newRefreshTokenEntity);
        await _refreshTokenRepo.AddAsync(newRefreshTokenEntity);

        return CommandResult.SuccessResult(new { AccessToken = accessToken, RefreshToken = newRefreshTokenEntity.Token, Username = user.Username, Role = user.RoleId });
    }

    public async Task<CommandResult> RevokeTokenAsync(string refreshToken)
    {
        var tokenEntity = (await _refreshTokenRepo.FindAsync(rt => rt.Token == refreshToken)).FirstOrDefault();
        if (tokenEntity == null) return CommandResult.FailureResult("Token not found");

        tokenEntity.Revoke();
        await _refreshTokenRepo.UpdateAsync(tokenEntity);

        return CommandResult.SuccessResult();
    }

    private string GenerateJwtToken(User user, out RefreshToken refreshTokenEntity)
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
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshTokenString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        refreshTokenEntity = RefreshToken.Create(user.Id, refreshTokenString, DateTime.UtcNow.AddDays(7));

        return tokenHandler.WriteToken(token);
    }
}
