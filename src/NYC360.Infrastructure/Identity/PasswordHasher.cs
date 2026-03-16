using NYC360.Application.Contracts.Authentication;
using System.Security.Cryptography;

namespace NYC360.Infrastructure.Identity;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;          // 128-bit
    private const int KeySize = 32;           // 256-bit
    private const int Iterations = 100_000;   // Safe baseline

    public string Hash(string input)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            input,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        var key = pbkdf2.GetBytes(KeySize);

        return Convert.ToBase64String(
            Combine(salt, key)
        );
    }

    public bool Verify(string input, string hash)
    {
        var decoded = Convert.FromBase64String(hash);

        var salt = decoded[..SaltSize];
        var key = decoded[SaltSize..];

        using var pbkdf2 = new Rfc2898DeriveBytes(
            input,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        var computedKey = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(
            key,
            computedKey
        );
    }

    private static byte[] Combine(byte[] first, byte[] second)
    {
        var result = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, result, 0, first.Length);
        Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
        return result;
    }
}