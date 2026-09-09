using System.ComponentModel.DataAnnotations;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Application.DTOs;

public class UpdateActionTaskDto
{
    [Required(ErrorMessage = "The type is required.")]
    public ActionType Type { get; set; }

    [Required(ErrorMessage = "The content is required.")]
    [MaxLength(500, ErrorMessage = "The content can't exceed 500 characters.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "The execution time is required.")]
    public DateTime ExecusionTime { get; set; }
}
