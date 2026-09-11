using SimpleAutomationEngine.Application.DTOs;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface IActionTaskService
{
    Task<ActionTaskResponseDto> CreateActionTaskAsync(CreateActionTaskDto dto);

    Task<List<ActionTaskResponseDto>> GetActionTasksAsync();

    Task<ActionTaskResponseDto> GetActionTaskByIdAsync(int id);

    Task<ActionTaskResponseDto> UpdateActionTaskAsync(
        int id,
        UpdateActionTaskDto dto);

    Task DeleteActionTaskAsync(int id);
}