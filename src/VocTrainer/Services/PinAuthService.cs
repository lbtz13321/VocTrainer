using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace VocTrainer.Services;

public class PinAuthState
{
    public bool IsAuthenticated { get; set; }
}

public class PinAuthService(IConfiguration config)
{
    private readonly string _pinHash = config["PinHash"]
        ?? throw new InvalidOperationException("PinHash is not configured.");

    private readonly ConcurrentDictionary<string, (int attempts, DateTime lockedUntil)> _rateLimits = new();

    private const int MaxAttempts = 10;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(1);

    public bool Verify(string pin, string clientIp)
    {
        if (IsLockedOut(clientIp))
            return false;

        var inputHash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(pin)));
        var storedHash = _pinHash.ToLowerInvariant();

        var match = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(inputHash),
            Encoding.UTF8.GetBytes(storedHash));

        if (match)
        {
            _rateLimits.TryRemove(clientIp, out _);
            return true;
        }

        RecordFailure(clientIp);
        return false;
    }

    public bool IsLockedOut(string clientIp)
    {
        if (!_rateLimits.TryGetValue(clientIp, out var entry))
            return false;

        if (entry.lockedUntil > DateTime.UtcNow)
            return true;

        if (entry.lockedUntil != DateTime.MinValue && entry.lockedUntil <= DateTime.UtcNow)
            _rateLimits.TryRemove(clientIp, out _);

        return false;
    }

    public TimeSpan GetRemainingLockout(string clientIp)
    {
        if (_rateLimits.TryGetValue(clientIp, out var entry) && entry.lockedUntil > DateTime.UtcNow)
            return entry.lockedUntil - DateTime.UtcNow;
        return TimeSpan.Zero;
    }

    private void RecordFailure(string clientIp)
    {
        _rateLimits.AddOrUpdate(clientIp,
            _ => (1, DateTime.MinValue),
            (_, existing) =>
            {
                var newAttempts = existing.attempts + 1;
                var locked = newAttempts >= MaxAttempts ? DateTime.UtcNow.Add(LockoutDuration) : DateTime.MinValue;
                return (newAttempts, locked);
            });
    }
}
