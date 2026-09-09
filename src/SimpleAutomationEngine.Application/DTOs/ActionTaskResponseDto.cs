using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Application.DTOs;

public class ActionTaskResponseDto
{
    public int ActionTaskId { get; set; }
    public ActionType Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime ExecusionTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public ActionStatus Status { get; set; }
}
