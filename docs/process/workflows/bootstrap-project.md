# Bootstrap проекта из шаблона

Чеклист первых шагов после клонирования. Время: ~2 часа.

## 1. Инициализация (15 мин)

```powershell
git clone <repo-url> my-project
cd my-project
.\scripts\init-project.ps1 -ProjectName "My Project" -ProjectPrefix "MP" -ProjectSlug "my-project"
```

Проверка: `.\scripts\verify.ps1`

## 2. Заполнить контекст (30 мин)

| Документ | Что заполнить |
|----------|---------------|
| [docs/process/context/containers.md](../context/containers.md) | Описание системы, идентификаторы домена |
| [docs/requirements/business/goals.md](../../requirements/business/goals.md) | Бизнес-цели |
| [docs/README.md](../../README.md) | Модель документации SPDF |
| [openspec/project.md](../../../openspec/project.md) | Purpose, domain context |
| `.cursorrules` | `project.domain`, `repository` |

## 3. Архитектура (20 мин)

- Обновить LikeC4: `docs/architecture/diagram/context/01-model.c4`
- Принять или изменить ADR в `docs/architecture/adr/`
- Заполнить constraints: `docs/requirements/constraints/`

## 4. Первая фича через OpenSpec (45 мин)

```text
/opsx-propose add-{entity}
```

Следовать [add-entity.md](add-entity.md):

1. Delta specs → OpenAPI → backend → frontend → tests
2. `npx openspec validate <id> --strict --no-interactive`
3. Реализация → `.\scripts\verify.ps1`
4. `/opsx-archive`

Эталоны: capability `examples` (CRUD), `categories` (read-only), `auth` (skeleton).

## 5. Окружение и DevOps

| Задача | Команда / файл |
|--------|----------------|
| Локальный запуск | `docker compose up --build` |
| Миграции | `Database:AutoMigrate=true` в Docker или `.\scripts\ef-migrate.ps1` |
| Мониторинг | `.\scripts\start-monitoring.ps1` |
| CI | `.github/workflows/ci.yml` |
| CD (шаблон) | `.github/workflows/cd.yml` |

## 6. Опционально

- `Auth:Enabled=true` + `VITE_AUTH_ENABLED=true` — проверить защиту POST
- `npm run generate:api` в frontend — типы из OpenAPI
- `npx likec4 validate` — диаграммы C4

## Антипаттерны

- ❌ Код без OpenSpec change для новой сущности
- ❌ Изменение OpenAPI без синхронного кода
- ❌ Оставить TBD в goals/containers при старте разработки
