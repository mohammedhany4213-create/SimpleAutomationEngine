using Microsoft.Extensions.Logging;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Application.Jobs;

public class ActionTaskExecutionJob
{
    private readonly IActionTaskRepository _repository;
    private readonly IActionExecutor _executor;
    private readonly ILogger<ActionTaskExecutionJob> _logger;

    public ActionTaskExecutionJob(
        IActionTaskRepository repository,
        IActionExecutor executor,
        ILogger<ActionTaskExecutionJob> logger)
    {
        _repository = repository;
        _executor = executor;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var dueTasks = await _repository.GetDueTasksAsync(DateTime.UtcNow);

        foreach (var task in dueTasks)
        {
            await ProcessTaskAsync(task);
        }
    }

    private async Task ProcessTaskAsync(ActionTask task)
    {
        
        var oldStatus = task.Status;
        task.Status = ActionStatus.Processing;
        await _repository.UpdateAsync(task);
        await _repository.AddLogAsync(new ActionLog
        {
            ActionTaskId = task.ActionTaskId,
            OldStatus = oldStatus,
            NewStatus = task.Status,
            Notes = "Picked up for execution."
        });

        try
        {
            await _executor.ExecuteAsync(task);

            var beforeExecuted = task.Status;
            task.Status = ActionStatus.Executed;
            await _repository.UpdateAsync(task);
            await _repository.AddLogAsync(new ActionLog
            {
                ActionTaskId = task.ActionTaskId,
                OldStatus = beforeExecuted,
                NewStatus = task.Status,
                Notes = "Executed successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute task {TaskId}", task.ActionTaskId);

            var beforeFailed = task.Status;
            task.Status = ActionStatus.Failed;
            await _repository.UpdateAsync(task);
            await _repository.AddLogAsync(new ActionLog
            {
                ActionTaskId = task.ActionTaskId,
                OldStatus = beforeFailed,
                NewStatus = task.Status,
                Notes = $"Execution failed: {ex.Message}"
            });
        }
    }
    public async Task RecoverStaleTasksAsync()
{
    var staleThreshold = DateTime.UtcNow.AddMinutes(-5);
    var staleTasks = await _repository.GetStaleProcessingTasksAsync(staleThreshold);

    foreach (var task in staleTasks)
    {
        _logger.LogWarning(
            "Task {TaskId} was stuck in Processing, reverting to Pending for retry.",
            task.ActionTaskId);

        var oldStatus = task.Status;
        task.Status = ActionStatus.Pending;
        await _repository.UpdateAsync(task);
        await _repository.AddLogAsync(new ActionLog
        {
            ActionTaskId = task.ActionTaskId,
            OldStatus = oldStatus,
            NewStatus = task.Status,
            Notes = "Recovered from a stale Processing state (likely a previous crash)."
        });
    }
}
}