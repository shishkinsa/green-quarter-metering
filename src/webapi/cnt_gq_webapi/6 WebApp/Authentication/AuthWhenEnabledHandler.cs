using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace GQ.WebApi.WebApp.Authentication;

/// <summary>
/// Пропускает запрос без аутентификации, если Auth отключён; иначе требует аутентифицированного пользователя.
/// </summary>
public sealed class AuthWhenEnabledHandler(IConfiguration configuration)
    : AuthorizationHandler<AuthWhenEnabledRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthWhenEnabledRequirement requirement)
    {
        if (!configuration.GetValue<bool>($"{AuthOptions.SectionName}:Enabled"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
