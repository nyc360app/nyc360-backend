namespace NYC360.Application.Contracts.Authentication;

public interface IPasswordHasher
{
    string Hash(string input);
    bool Verify(string input, string hash);
}