using Microsoft.AspNetCore.Mvc;
using SimpleAutomationEngine.Application.DTOs;
using SimpleAutomationEngine.Application.Interfaces;

namespace SimpleAutomationEngine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var createDto = new CreateUserDto
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            var result = await _userService.RegisterAsync(createDto, dto.Password);
            return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto)
{
    try
    {
        var result = await _userService.LoginAsync(dto);
        return Ok(result);
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
}

[HttpPost("refresh")]
public async Task<IActionResult> Refresh(RefreshTokenDto dto)
{
    try
    {
        var result = await _userService.RefreshTokenAsync(dto);
        return Ok(result);
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
}
}