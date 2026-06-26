namespace GQ.WebApi.WebApp.Authentication;

/// <summary>
/// Настройки аутентификации API.
/// </summary>
public sealed class AuthOptions
{
    public const string SectionName = "Auth";

    /// <summary>
    /// Включить проверку JWT на защищённых эндпоинтах.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Issuer OIDC/JWT (для production).
    /// </summary>
    public string? Authority { get; init; }

    /// <summary>
    /// Audience токена.
    /// </summary>
    public string Audience { get; init; } = "gq-webapi";
}
