## Why

Текущий frontend разделён на две несвязанные страницы («Справочники» и «Показания») с устаревшим UX: inline-формы, нет иерархической навигации дом → квартира → показания. Для MVP нужен единый сценарий работы энергетика: стартовая таблица домов, drill-down по клику на строку, CRUD через модальные окна.

Параллельно сохраняются цели FSD-рефакторинга: типы из OpenAPI, тонкие pages, features по действию, shared-слой.

## What Changes

### Навигация и экраны (frontend)

- **Стартовая страница** `/` — таблица домов; кнопка «Добавить дом» → модальная форма; действия в строке: редактировать, удалить; клик по строке → квартиры дома
- **Страница квартир** `/buildings/:buildingId/apartments` — таблица: id, номер, владелец, последняя дата передачи, показание, статус передачи; «Добавить квартиру» → модалка; клик по строке → история показаний квартиры
- **Страница показаний** `/apartments/:apartmentId/meter-readings` — таблица: год, месяц, показание, дата и время передачи; «Передать показание» → модалка
- Удаление страниц `/directories` и `/meter-readings` как отдельных пунктов меню; единая точка входа
- Модальные формы для create/update/delete (Element Plus `ElDialog`)
- FSD: features (`create-building`, `update-building`, `delete-building`, `create-apartment`, `submit-meter-reading` и т.д.), composables, типы из OpenAPI

### API (минимальные расширения для UI)

- `DELETE /buildings/{buildingId}` — удаление дома
- `DELETE /apartments/{apartmentId}` — удаление квартиры
- `GET /apartments/{apartmentId}/meter-readings` — история показаний квартиры
- Поле `submittedAt` в `MeterReadingDto` — дата и время передачи
- Расширение ответа списка квартир дома полями сводки по показаниям (последняя дата, значение, статус текущего периода)

## Capabilities

### New Capabilities

- `frontend-fsd`: SPA `cnt_gq_web` — иерархическая навигация, таблицы и модальные формы, FSD-структура

### Modified Capabilities

- `directories`: удаление дома и квартиры; сводка показаний в списке квартир
- `meter-readings`: история показаний по квартире; `submittedAt` при передаче

## Impact

- Frontend: `src/frontend/cnt_gq_web/src/**` — pages, features, entities, widgets, app/router
- Backend: handlers delete, list apartment readings, migration `SubmittedAt`, расширение DTO квартир
- OpenAPI: v0.8.0 — новые endpoints и поля
- Docs: `docs/architecture/specs/frontend.md`, `manifest.yaml`
- Удаляются или заменяются `pages/directories`, `pages/meter-readings` в текущем виде
