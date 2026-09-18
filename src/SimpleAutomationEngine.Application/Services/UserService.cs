using SimpleAutomationEngine.Application.DTOs;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;

namespace SimpleAutomationEngine.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;

    public UserService(IUserRepository repository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
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

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(7);
        await _repository.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
{
    var user = await _repository.GetByRefreshTokenAsync(dto.RefreshToken);

    if (user is null
        || user.RefreshTokenExpirationTime is null
        || user.RefreshTokenExpirationTime <= DateTime.UtcNow)
    {
        throw new UnauthorizedAccessException("Invalid or expired refresh token.");
    }

    var newAccessToken = _tokenGenerator.GenerateAccessToken(user);
    var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(7);
    await _repository.UpdateAsync(user);

    return new AuthResponseDto
    {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken
    };
}
}
