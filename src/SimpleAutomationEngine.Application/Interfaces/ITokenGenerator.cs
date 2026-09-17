using SimpleAutomationEngine.Domain.Entities;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface ITokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}