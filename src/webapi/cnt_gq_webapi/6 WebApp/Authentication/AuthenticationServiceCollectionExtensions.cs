using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GQ.WebApi.WebApp.Authentication;

/// <summary>
/// Регистрация аутентификации API.
/// </summary>
public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddGqAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.SectionName));

        var authEnabled = configuration.GetValue<bool>($"{AuthOptions.SectionName}:Enabled");

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthPolicies.AuthenticatedWhenEnabled,
                policy => policy.Requirements.Add(new AuthWhenEnabledRequirement()));
        });
        services.AddSingleton<IAuthorizationHandler, AuthWhenEnabledHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthFailureResultHandler>();

        if (!authEnabled)
        {
            return services;
        }

        var authBuilder = services.AddAuthentication(options =>
        {
            if (environment.IsDevelopment())
            {
                options.DefaultAuthenticateScheme = DevBearerAuthenticationHandler.SchemeName;
                options.DefaultChallengeScheme = DevBearerAuthenticationHandler.SchemeName;
            }
            else
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        });

        if (environment.IsDevelopment())
        {
            authBuilder.AddScheme<AuthenticationSchemeOptions, DevBearerAuthenticationHandler>(
                DevBearerAuthenticationHandler.SchemeName,
                _ => { });
        }
        else
        {
            var authOptions = configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>() ?? new AuthOptions();
            authBuilder.AddJwtBearer(options =>
            {
                options.Authority = authOptions.Authority;
                options.Audience = authOptions.Audience;
                options.RequireHttpsMetadata = true;
            });
        }

        return services;
    }
}
