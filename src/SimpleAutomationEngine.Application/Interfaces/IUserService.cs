using SimpleAutomationEngine.Application.DTOs ;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface IUserService
{
    public Task<UserResponseDto> RegisterAsync(CreateUserDto dto , string password);
}