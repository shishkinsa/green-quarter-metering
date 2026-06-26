using Microsoft.EntityFrameworkCore;
using GQ.Shared.Observability.Extensions;
using GQ.WebApi.DataAccess.Postgres.Data;
using GQ.WebApi.DataAccess.Postgres.DependencyInjection;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Validators;
using GQ.WebApi.WebApp.Authentication;
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

builder.Services.AddGqAuthentication(builder.Configuration, builder.Environment);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddPostgresDataAccess(connectionString);
}

builder.Services.AddValidatorsFromAssemblyContaining<CreateExampleCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateExampleCommand).Assembly));

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
    DatabaseSeeder.SeedCategories(db);
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var authEnabled = builder.Configuration.GetValue<bool>("Auth:Enabled");
if (authEnabled)
{
    app.UseAuthentication();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
