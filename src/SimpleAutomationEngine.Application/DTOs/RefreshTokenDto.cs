using System.ComponentModel.DataAnnotations;

namespace SimpleAutomationEngine.Application.DTOs;

public class RefreshTokenDto
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = string.Empty;
}