namespace BizI.Domain.Entities;

using System;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, DateTime expiryDate)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiryDate = expiryDate,
            IsRevoked = false
        };
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        Touch();
    }
}
