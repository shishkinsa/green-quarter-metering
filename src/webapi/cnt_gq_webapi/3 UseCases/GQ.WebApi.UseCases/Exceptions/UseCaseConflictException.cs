namespace GQ.WebApi.UseCases.Exceptions;

/// <summary>
/// Конфликт доменных ограничений (например, дубликат ключа).
/// </summary>
public sealed class UseCaseConflictException(string message): Exception(message);
