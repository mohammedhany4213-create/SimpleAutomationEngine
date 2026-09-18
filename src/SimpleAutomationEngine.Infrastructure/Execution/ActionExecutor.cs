using Microsoft.Extensions.Logging;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Infrastructure.Execution;

public class ActionExecutor : IActionExecutor
{
    private readonly ILogger<ActionExecutor> _logger;

    public ActionExecutor(ILogger<ActionExecutor> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync(ActionTask task)
    {
        switch (task.Type)
        {
            case ActionType.Email:
                // TODO: هنا هنستبدل السطر ده بإرسال إيميل حقيقي (SMTP/SendGrid) في خطوة لاحقة
                _logger.LogInformation(
                    "Sending email for task {TaskId} to user {UserId}: {Content}",
                    task.ActionTaskId, task.UserId, task.Content);
                break;

            case ActionType.Notification:
                // TODO: هنا هنستبدل السطر ده بإرسال notification حقيقي
                _logger.LogInformation(
                    "Sending notification for task {TaskId} to user {UserId}: {Content}",
                    task.ActionTaskId, task.UserId, task.Content);
                break;

            default:
                throw new InvalidOperationException($"Unknown action type: {task.Type}");
        }

        await Task.CompletedTask;
    }
}