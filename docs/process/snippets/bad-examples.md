# Антипаттерны для AI

Примеры того, **как не делать** в этом проекте.

## Backend: бизнес-логика в контроллере

```csharp
// ❌ Плохо
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateBuildingRequest request)
{
    var entity = new Building { Id = Guid.NewGuid(), Name = request.Name };
    await _dbContext.Set<Building>().AddAsync(entity);
    await _dbContext.SaveChangesAsync();
    return Ok(entity);
}
```

Логика — в `CommandHandler`, контроллер только диспетчеризует MediatR.

## Backend: прямой AppDbContext в UseCases

```csharp
// ❌ Плохо — UseCases не ссылается на Infrastructure.Implementation
public class CreateBuildingCommandHandler(AppDbContext db) { ... }
```

Используй `IDbContext` из `Infrastructure.Interfaces`; сложные выборки — `I*Queries`.

## Backend: изменение API без OpenAPI

Добавление поля в response без обновления `docs/architecture/openapi/components/openapi.yaml` — запрещено.

## Frontend: fetch в компоненте страницы

```typescript
// ❌ Плохо — в pages/directories/ui/DirectoriesPage.vue
onMounted(() => {
  fetch('/api/v1/buildings').then(r => r.json()).then(setData);
});
```

API-вызовы — в `entities/*/api`, состояние — в `features/*/model` или composables фичи.

## Frontend: импорт features из entities

```typescript
// ❌ Плохо — нарушение FSD
import { CreateBuildingForm } from '@/features/directory/create-building';
// внутри entities/building/
```

Зависимости FSD: `app → pages → widgets → features → entities → shared`.

## Архитектура: новый контейнер без ADR

Добавление Redis, брокера или Identity без ADR и обновления LikeC4 / `context/containers.md`.
