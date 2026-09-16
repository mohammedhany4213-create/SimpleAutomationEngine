using BCrypt.Net ;
using SimpleAutomationEngine.Application.Interfaces;

namespace SimpleAutomationEngine.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password , hashedPassword);
    }
}