using SimpleAutomationEngine.Application.DTOs ;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> RegisterAsync(CreateUserDto dto, string password);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}