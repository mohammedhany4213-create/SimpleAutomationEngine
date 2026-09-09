using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Domain.Entities;
using SimpleAutomationEngine.Domain.Enums ;

namespace SimpleAutomationEngine.Infrastructure.Data ;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){

    }

    public DbSet<User> Users {get; set;} = null! ;
    public DbSet<ActionTask> Actions {get; set;} = null! ;

    public DbSet<ActionLog> ActionLogs {get; set;} = null! ;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Unique Email
        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
        
        //change the status to string from enum
        modelBuilder.Entity<ActionTask>()
        .Property(a => a.Status)
        .HasConversion<string>();
        
        //change the type to string from enum
        modelBuilder.Entity<ActionTask>()
        .Property(a => a.Type)
        .HasConversion<string>();

        //Delete all the behaviors when delete user
        modelBuilder.Entity<ActionTask>()
        .HasOne(a => a.User)
        .WithMany(a => a.ActionTasks)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        //change the status to string from enum in Action log

        modelBuilder.Entity<ActionLog>()
        .Property(o => o.OldStatus)
        .HasConversion<string>();

        modelBuilder.Entity<ActionLog>()
        .Property(o => o.NewStatus)
        .HasConversion<string>();

        //delete all the behaviors after deleting user

        modelBuilder.Entity<ActionLog>()
        .HasOne(l => l.ActionTask)
        .WithMany(a => a.ActionLogs)
        .HasForeignKey(a => a.ActionTaskId)
        .OnDelete(DeleteBehavior.Cascade);
        


    }
}