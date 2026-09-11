using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Infrastructure.Data ;
using SimpleAutomationEngine.Infrastructure.Repositories;
using SimpleAutomationEngine.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IActionTaskRepository , ActionTaskRepository>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build() ;

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();