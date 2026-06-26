# Примеры кода для AI

Few-shot примеры «хорошего» стиля проекта. Дополняйте по мере появления эталонных реализаций.

## Backend: обработчик команды (Requestum)

```csharp
using Requestum.Contract;

namespace GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;

public sealed record CreateExampleCommand(string Name) : ICommand<CreateExampleResponse>;

public sealed class CreateExampleCommandHandler(IExampleItemRepository repository)
    : IAsyncCommandHandler<CreateExampleCommand, CreateExampleResponse>
{
    public async Task<CreateExampleResponse> ExecuteAsync(
        CreateExampleCommand command,
        CancellationToken cancellationToken = default)
    {
        var entity = ExampleItem.Create(command.Name);
        await repository.AddAsync(entity, cancellationToken);
        return new CreateExampleResponse(ExampleMappings.ToDto(entity));
    }
}
```

## Frontend: хук фичи

```typescript
/**
 * Загружает список элементов для экрана примера.
 *
 * @returns состояние загрузки и данные
 */
export function useLoadExamples() {
  // ...
}
```
