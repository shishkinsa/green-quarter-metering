# Контейнеры (C4) — краткая сводка для AI

**Зелёный квартал — Учёт электроэнергии** — система учёта электроэнергии ЖК. Ответственный: энергетик Василий.

Полная модель: [diagram/context/01-model.c4](../../architecture/diagram/context/01-model.c4). При изменении архитектуры — сначала LikeC4, затем эта таблица.

| Идентификатор | Тип | Технология | Назначение |
| --- | --- | --- | --- |
| CNT_GQ_Web | Клиентское приложение | Vue 3, TypeScript, Vite, Element Plus | SPA: UI, вызовы REST API |
| CNT_GQ_WebAPI | Сервис API | ASP.NET Core 8, MediatR, EF Core | REST API: бизнес-логика, PostgreSQL |
| CNT_GQ_DB | База данных | PostgreSQL | Транзакционные данные приложения |

**Поток запросов:** `CNT_GQ_Web` → `CNT_GQ_WebAPI` → `CNT_GQ_DB`

**Идентификаторы домена:** UUID для сущностей приложения; коды справочников — string.

См. также: [README.md](../../README.md), [manifest.yaml](../../../manifest.yaml).
