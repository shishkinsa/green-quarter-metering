namespace GQ.WebApi.UseCases.Exceptions;

/// <summary>
/// Сущность или ресурс сценария не найдены.
/// </summary>
public sealed class UseCaseNotFoundException(string message): Exception(message);
