# CNT_GQ_DB: структура метаданных

База данных PostgreSQL для `CNT_GQ_WebAPI`. Идентификаторы доменных сущностей — **UUID**.

> **Именование колонок:** EF Core по умолчанию создаёт колонки в **PascalCase** (`Id`, `Name`). При необходимости snake_case — настройте `UseSnakeCaseNamingConvention()` и обновите миграции.

## Таблица `buildings`

| Колонка (EF) | Тип PostgreSQL | Ограничения | Описание |
|--------------|----------------|-------------|----------|
| `Id` | `uuid` | PK | Идентификатор |
| `Name` | `varchar(256)` | NOT NULL | Наименование дома |
| `Address` | `varchar(512)` | NULL | Адрес |

## Таблица `apartments`

| Колонка (EF) | Тип PostgreSQL | Ограничения | Описание |
|--------------|----------------|-------------|----------|
| `Id` | `uuid` | PK | Идентификатор |
| `BuildingId` | `uuid` | FK → `buildings` | Дом |
| `Number` | `varchar(32)` | NOT NULL | Номер квартиры |
| `Floor` | `int` | NULL | Этаж |

Уникальный индекс: (`BuildingId`, `Number`).

## Таблица `owners`

| Колонка (EF) | Тип PostgreSQL | Ограничения | Описание |
|--------------|----------------|-------------|----------|
| `Id` | `uuid` | PK | Идентификатор |
| `ApartmentId` | `uuid` | FK → `apartments`, UNIQUE | Квартира |
| `FullName` | `varchar(256)` | NOT NULL | ФИО владельца |
| `Phone` | `varchar(32)` | NULL | Телефон |

Seed: `DatabaseSeeder.SeedDirectories` (при старте и в тестах).

Миграции EF Core: `src/webapi/cnt_gq_webapi/5 Infrastructure.Implementation/GQ.WebApi.DataAccess.Postgres/Migrations/`
