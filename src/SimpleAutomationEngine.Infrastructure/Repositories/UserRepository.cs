using SimpleAutomationEngine.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Infrastructure.Data;

public class UserRepository : IUserRepository
{

    private readonly AppDbContext _context ;
    public UserRepository(AppDbContext context)
    {
        _context = context ;
    }
    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user ;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task UpdateAsync(User user)
{
    _context.Users.Update(user);
    await _context.SaveChangesAsync();
}
}