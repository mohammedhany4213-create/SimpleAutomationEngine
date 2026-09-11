using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Infrastructure.Data;

namespace SimpleAutomationEngine.Infrastructure.Repositories;

public class ActionTaskRepository : IActionTaskRepository
{
    private readonly AppDbContext _context ;

    ActionTaskRepository(AppDbContext context)
    {
        _context = context ;
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
}