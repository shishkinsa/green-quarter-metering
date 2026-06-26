using Microsoft.EntityFrameworkCore;
using GQ.Shared.Observability.Extensions;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.DependencyInjection;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;
using GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding.Validators;
using GQ.WebApi.WebApp.ExceptionHandlers;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var serviceName = "GQ.WebApi.WebApp";
var serviceVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.1.0";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
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

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("Database:AutoMigrate")
    && !string.IsNullOrWhiteSpace(connectionString))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DatabaseSeeder.SeedDirectories(db);
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program;
