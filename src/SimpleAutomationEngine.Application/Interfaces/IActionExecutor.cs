using SimpleAutomationEngine.Domain.Entities;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface IActionExecutor
{
    Task ExecuteAsync(ActionTask task);
}