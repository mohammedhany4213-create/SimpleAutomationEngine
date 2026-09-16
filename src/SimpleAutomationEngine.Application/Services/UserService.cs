using SimpleAutomationEngine.Application.DTOs;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;

namespace SimpleAutomationEngine.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseDto> RegisterAsync(CreateUserDto dto, string password)
    {
        var existingUser = await _repository.GetByEmailAsync(dto.Email);
        if (existingUser is not null)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(password)
        };

        var created = await _repository.AddAsync(user);

        return new UserResponseDto
        {
            UserId = created.UserId,
            FullName = created.FullName,
            Email = created.Email,
            CreatedAt = created.CreatedAt
        };
    }
}
