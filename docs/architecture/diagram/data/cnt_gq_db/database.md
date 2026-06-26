# CNT_GQ_DB: структура метаданных

База данных PostgreSQL для `CNT_GQ_WebAPI`. Идентификаторы доменных сущностей — **UUID**.

> **Именование колонок:** EF Core по умолчанию создаёт колонки в **PascalCase** (`Id`, `Name`). При необходимости snake_case — настройте `UseSnakeCaseNamingConvention()` и обновите миграции.

## Таблица `example_items`

| Колонка (EF) | Тип PostgreSQL | Ограничения | Описание |
|--------------|----------------|-------------|----------|
| `Id` | `uuid` | PK | Идентификатор |
| `Name` | `varchar(256)` | NOT NULL | Наименование |
| `CreatedAt` | `timestamptz` | NOT NULL | Время создания (UTC) |

## Таблица `categories` (read-only справочник)

| Колонка (EF) | Тип PostgreSQL | Ограничения | Описание |
|--------------|----------------|-------------|----------|
| `Id` | `uuid` | PK | Идентификатор |
| `Code` | `varchar(64)` | NOT NULL | Код категории |
| `Name` | `varchar(256)` | NOT NULL | Наименование |

Seed: `DatabaseSeeder.SeedCategories` (при старте и в тестах).

Миграции EF Core: `src/webapi/cnt_gq_webapi/5 Infrastructure.Implementation/GQ.WebApi.DataAccess.Postgres/Migrations/`
