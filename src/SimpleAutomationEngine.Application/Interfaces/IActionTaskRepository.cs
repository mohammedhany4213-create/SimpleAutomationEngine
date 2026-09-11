using SimpleAutomationEngine.Domain.Entities ;

namespace SimpleAutomationEngine.Application.Interfaces ;

public interface IActionTaskRepository
{
    Task<ActionTask> AddAsync(ActionTask task);
    Task<ActionTask?> GetByIdAsync(int id);
    Task<List<ActionTask>> GetAllAsync();
    Task UpdateAsync(ActionTask task);
    Task DeleteAsync(ActionTask task);
    
}