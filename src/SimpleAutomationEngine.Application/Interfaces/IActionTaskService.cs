using SimpleAutomationEngine.Application.DTOs;

namespace SimpleAutomationEngine.Application.Interfaces;

public interface IActionTaskService
{
    Task<ActionTaskResponseDto> CreateActionTaskAsync(CreateActionTaskDto dto, int userId);

    Task<List<ActionTaskResponseDto>> GetAllAsync(int userId);

    Task<ActionTaskResponseDto?> GetByIdAsync(int id, int userId);

    Task<ActionTaskResponseDto> UpdateActionTaskAsync(int id, UpdateActionTaskDto dto, int userId);
    Task DeleteActionTaskAsync(int id, int userId);

    Task<ActionTaskResponseDto> CancelActionTaskAsync(int id, int userId);
}