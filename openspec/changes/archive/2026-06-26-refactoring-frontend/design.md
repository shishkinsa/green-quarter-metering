## Context

SPA `cnt_gq_web` (Vue 3 + Element Plus + FSD) сейчас имеет две изолированные страницы: `/directories` и `/meter-readings`. Целевой UX — иерархия **дома → квартиры → показания** с таблицами и модальными формами, как описано в assignment SA.

Текущий API не покрывает: удаление дома/квартиры, историю показаний по квартире, `submittedAt`, сводку показаний в списке квартир. Change становится **full-stack** с приоритетом frontend-требований.

## Goals / Non-Goals

**Goals:**

- Единая стартовая страница с таблицей домов (`/`)
- Drill-down: клик по строке → квартиры → показания
- Все мутации через `ElDialog` (create/update/delete/submit)
- FSD: features по действию, composables, типы OpenAPI, тонкие pages
- Хлебные крошки и nested routes в `AppLayout`
- Минимальные API-расширения под колонки таблиц

**Non-Goals:**

- Смена UI-библиотеки (остаёмся на Element Plus)
- Сводка сдачи по дому за период как отдельный экран (endpoint `GET /buildings/{id}/meter-readings` сохраняется, но отдельная страница не нужна)
- Pinia/Vuex
- E2e Playwright
- Редактирование/удаление отдельных показаний из истории

## Decisions

### 1. Маршруты

```text
/ (AppLayout)
  ├── /                              → BuildingsPage (таблица домов)
  ├── /buildings/:buildingId/apartments → ApartmentsPage
  └── /apartments/:apartmentId/meter-readings → ApartmentMeterReadingsPage
```

Старые `/directories` и `/meter-readings` удаляются; редирект `/directories` → `/` для совместимости закладок (опционально).

### 2. Модальные формы

**Решение:** каждая feature экспортирует composable + компонент диалога (`CreateBuildingDialog.vue`, `SubmitMeterReadingDialog.vue`).

**Причина:** единообразие UX; page только монтирует feature и передаёт `@success` для refresh таблицы.

### 3. Клик по строке vs действия

**Решение:** `@row-click` на `ElTable`; кнопки действий с `@click.stop`.

### 4. Сводка показаний в списке квартир

**Решение:** расширить `ApartmentWithOwnerDto` полями `lastReadingSubmittedAt`, `lastReadingValue`, `currentPeriodSubmitted`; вычислять в `ListApartmentsWithOwnersQueryHandler` одним SQL/join.

**Альтернатива:** два запроса на frontend — отклонена: лишний round-trip и сложность merge.

**`currentPeriodSubmitted`:** текущий календарный год/месяц сервера (UTC).

### 5. `submittedAt`

**Решение:** колонка `SubmittedAt timestamptz` в `meter_readings`, заполняется при create/update; возвращается в `MeterReadingDto`.

### 6. Удаление

**Решение:** `DELETE` endpoints с каскадом через EF Core или явное удаление связей в handler.

**Подтверждение:** `ElMessageBox.confirm` в feature `delete-building` / `delete-apartment`.

### 7. FSD features

| Feature | Действие |
|---------|----------|
| `create-building` | модалка создания дома |
| `update-building` | модалка редактирования |
| `delete-building` | confirm + DELETE |
| `create-apartment` | модалка создания квартиры |
| `delete-apartment` | confirm + DELETE |
| `upsert-apartment-owner` | модалка владельца |
| `submit-meter-reading` | модалка передачи |

### 8. Pages и entities

- `pages/buildings` — таблица домов
- `pages/apartments` — таблица квартир (param `buildingId`)
- `pages/apartment-meter-readings` — история показаний
- `entities/building`, `entities/apartment`, `entities/meter-reading` — API + composables + presentation tables

### 9. OpenAPI v0.8.0

Новые paths и поля синхронно с backend до `generate:api`.

## Risks / Trade-offs

- [Breaking URLs] → редирект `/directories` → `/`
- [Каскадное удаление] → явное подтверждение в UI; документировать в API
- [Большой scope] → tasks разбиты: API → frontend pages → FSD cleanup
- [Статус «текущий месяц»] → привязан к UTC сервера; задокументировать

## Migration Plan

1. OpenAPI + backend (delete, list readings, submittedAt, apartment summary)
2. `npm run generate:api`
3. Новые pages и features с модалками
4. Удаление старых pages, обновление router и layout
5. FSD cleanup (shared errors, ESLint, README)
6. `verify.ps1`

Rollback: revert по коммитам; миграция `SubmittedAt` обратима при откате до deploy.

## Open Questions

- Нужен ли редирект `/meter-readings` → `/` или достаточно удалить маршрут?
- Редактирование квартиры (номер/этаж) — в scope или только create/delete?
