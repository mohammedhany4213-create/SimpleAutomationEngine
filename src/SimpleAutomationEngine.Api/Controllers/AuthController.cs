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
}