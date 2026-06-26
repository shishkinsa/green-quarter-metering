using FluentValidation;

using GQ.Shared.Observability.Extensions;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.DependencyInjection;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding.Validators;
using GQ.WebApi.WebApp.ExceptionHandlers;

using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var serviceName = "GQ.WebApi.WebApp";
string serviceVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.1.0";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if(!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddPostgresDataAccess(connectionString);
}

builder.Services.AddValidatorsFromAssemblyContaining<CreateBuildingCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBuildingCommand).Assembly));

builder.Services.AddGQObservability(
    builder.Logging,
    builder.Configuration,
    serviceName,
    serviceVersion);

WebApplication app = builder.Build();

if(builder.Configuration.GetValue<bool>("Database:AutoMigrate")
    && !string.IsNullOrWhiteSpace(connectionString))
{
    using IServiceScope scope = app.Services.CreateScope();
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if(db.Database.IsRelational())
    {
        db.Database.Migrate();
    }
}

app.UseExceptionHandler();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

/// <summary>Точка входа Web API; partial-класс для интеграционных тестов (<c>WebApplicationFactory</c>).</summary>
public partial class Program;
