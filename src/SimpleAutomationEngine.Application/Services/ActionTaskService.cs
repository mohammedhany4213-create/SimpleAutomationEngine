using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Domain.Enums;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Application.DTOs;

namespace SimpleAutomationEngine.Application.Services;

public class ActionTaskService : IActionTaskService
{
    private readonly IActionTaskRepository _repository ;
    ActionTaskService(IActionTaskRepository repository)
    {
        _repository = repository ;
    }

    private static ActionTaskResponseDto MapToResponseDto(ActionTask task)
    {
        return new ActionTaskResponseDto
        {
            ActionTaskId = task.ActionTaskId ,
            Type = task.Type ,
            Content = task.Content ,
            ExecutionTime = task.ExecutionTime ,
            CreatedAt = task.CreatedAt ,
            Status = task.Status
        };
    }


    public async Task<ActionTaskResponseDto> CreateActionTaskAsync(CreateActionTaskDto dto)
    {
        var task = new ActionTask
        {
            Type = dto.Type ,
            Content = dto.Content ,
            ExecutionTime = dto.ExecutionTime ,
            Status = ActionStatus.Pending 
        };

        var created = await _repository.AddAsync(task);
        return MapToResponseDto(created);

    }

    public async Task DeleteActionTaskAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task is null)
            throw new KeyNotFoundException($"Action task with id {id} was not found.");
        
        else
        await _repository.DeleteAsync(task);

    }


    public async Task<ActionTaskResponseDto> GetActionTaskByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if(task is null)
            throw new KeyNotFoundException($"Action task with id {id} was not found. ");
        else
        return MapToResponseDto(task);
    }

    public async Task<List<ActionTaskResponseDto>> GetActionTasksAsync()
    {
        List<ActionTask>tasks = await _repository.GetAllAsync();
        return tasks.Select(MapToResponseDto).ToList();


    }

    public async Task<ActionTaskResponseDto> UpdateActionTaskAsync(int id, UpdateActionTaskDto dto)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            throw new KeyNotFoundException($"Action task with id {id} was not found.");
        task.Type = dto.Type;
        task.Content = dto.Content ;
        task.ExecutionTime = dto.ExecutionTime;

        await _repository.UpdateAsync(task);

        return MapToResponseDto(task);
        
    }
}