
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mainConnectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(mainConnectionString))
{
    throw new InvalidOperationException(
        "Main database connection string is missing.");
}

var workerConnectionString = builder.Configuration
    .GetConnectionString("WorkerAuthConnection");

if (string.IsNullOrWhiteSpace(workerConnectionString))
{
    throw new InvalidOperationException(
        "Worker authentication database connection string is missing.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(mainConnectionString));

builder.Services.AddDbContext<WorkerAuthDbContext>(options =>
    options.UseSqlServer(workerConnectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDevelopment", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendDevelopment");

app.MapGet("/api/health", () =>
    Results.Ok(new
    {
        status = "ok",
        application = "UrbanSignals.Api"
    }));

app.MapControllers();

app.Run();

public partial class Program { }
