# Примеры кода для AI

Few-shot примеры «хорошего» стиля проекта. Дополняйте по мере появления эталонных реализаций.

## Backend: обработчик команды (MediatR)

```csharp
namespace GQ.WebApi.UseCases.Handlers.Building.Commands.CreateBuilding;

public sealed class CreateBuildingCommandHandler(IDbContext db)
    : IRequestHandler<CreateBuildingCommand, CreateBuildingResponse>
{
    public async Task<CreateBuildingResponse> Handle(
        CreateBuildingCommand command,
        CancellationToken cancellationToken = default)
    {
        var entity = Building.Create(command.Name, command.Address);
        db.Buildings.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        return new CreateBuildingResponse(BuildingMappings.ToDto(entity));
    }
}
```

## Frontend: API-слой entity

```typescript
/**
 * Загружает список домов для страницы справочников.
 */
export async function fetchBuildings(): Promise<ListBuildingsResponse> {
  return apiFetch<ListBuildingsResponse>('/v1/buildings');
}
```
