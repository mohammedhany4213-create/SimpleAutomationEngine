using System.ComponentModel.DataAnnotations;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Application.DTOs;

public class UpdateActionTaskDto : IValidatableObject
{
    [EnumDataType(typeof(ActionType), ErrorMessage = "Invalid action type.")]
    public ActionType Type { get; set; }

    [Required(ErrorMessage = "The content is required.")]
    [MaxLength(500, ErrorMessage = "The content can't exceed 500 characters.")]
    public string Content { get; set; } = string.Empty;

    public DateTime ExecutionTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Content))
        {
            yield return new ValidationResult(
                "The content can't be empty or whitespace.",
                new[] { nameof(Content) });
        }

        if (ExecutionTime == default)
        {
            yield return new ValidationResult(
                "The execution time is required.",
                new[] { nameof(ExecutionTime) });
        }
        else if (ExecutionTime <= DateTime.UtcNow)
        {
            yield return new ValidationResult(
                "The execution time must be in the future.",
                new[] { nameof(ExecutionTime) });
        }
    }
}
