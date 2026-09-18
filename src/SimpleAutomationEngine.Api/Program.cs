using Microsoft.EntityFrameworkCore;
using SimpleAutomationEngine.Infrastructure.Data;
using SimpleAutomationEngine.Infrastructure.Repositories;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Application.Services;
using SimpleAutomationEngine.Infrastructure.Security;
using SimpleAutomationEngine.Api.Settings;
using SimpleAutomationEngine.Api.Security;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire ;
using SimpleAutomationEngine.Infrastructure.Execution;
using SimpleAutomationEngine.Application.Jobs;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
);

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IActionTaskRepository, ActionTaskRepository>();
builder.Services.AddScoped<IActionTaskService, ActionTaskService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActionExecutor, ActionExecutor>();
builder.Services.AddScoped<ActionTaskExecutionJob>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SimpleAutomationEngine API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "اكتب التوكن هنا بس (من غير كلمة Bearer قبله)"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireDashboardAuthFilter() }
});
RecurringJob.AddOrUpdate<ActionTaskExecutionJob>(
    "execute-due-action-tasks",
    job => job.RunAsync(),
    "* * * * *");
    
    RecurringJob.AddOrUpdate<ActionTaskExecutionJob>(
    "recover-stale-processing-tasks",
    job => job.RecoverStaleTasksAsync(),
    "*/5 * * * *");


app.MapControllers();

app.Run();