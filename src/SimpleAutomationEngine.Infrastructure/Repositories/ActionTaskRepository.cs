using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Infrastructure.Data;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Infrastructure.Repositories;

public class ActionTaskRepository : IActionTaskRepository
{
    private readonly AppDbContext _context ;

    public ActionTaskRepository(AppDbContext context)
    {
        _context = context ;
    }

    public async Task<List<ActionTask>> GetAllByUserIdAsync(int userId)
{
    return await _context.Actions
        .Where(t => t.UserId == userId)
        .ToListAsync();
}
    public async Task<ActionTask> AddAsync(ActionTask task)
    {
        _context.Actions.Add(task);
        await _context.SaveChangesAsync();
        return task ;
    }

    public async Task DeleteAsync(ActionTask task)
    {
        _context.Actions.Remove(task);
        await _context.SaveChangesAsync();

    }

    public async Task<List<ActionTask>> GetAllAsync()
    {
        return await _context.Actions.ToListAsync();
    }

    public async Task<ActionTask?> GetByIdAsync(int id)
    {
        return await _context.Actions.FindAsync(id);
    }

    public async Task UpdateAsync(ActionTask task)
    {
        _context.Actions.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task AddLogAsync(ActionLog log)
{
    _context.ActionLogs.Add(log);
    await _context.SaveChangesAsync();
}

public async Task<List<ActionTask>> GetDueTasksAsync(DateTime now)
{
    return await _context.Actions
        .Where(t => t.Status == ActionStatus.Pending && t.ExecutionTime <= now)
        .ToListAsync();
}

public async Task<List<ActionTask>> GetStaleProcessingTasksAsync(DateTime staleBefore)
{
    return await _context.Actions
        .Where(t => t.Status == ActionStatus.Processing)
        .Where(t => t.ActionLogs
            .Where(l => l.NewStatus == ActionStatus.Processing)
            .OrderByDescending(l => l.Timestamp)
            .Select(l => l.Timestamp)
            .FirstOrDefault() <= staleBefore)
        .ToListAsync();
}

}