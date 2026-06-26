## 1. Spec & Contract

- [x] 1.1 Обновить OpenAPI v0.8.0: DELETE building/apartment, GET apartment meter-readings, `submittedAt`, поля сводки в `ApartmentWithOwnerDto`
- [x] 1.2 `npx openspec validate refactoring-frontend --strict --no-interactive`

## 2. Backend — directories

- [x] 2.1 `DeleteBuildingCommandHandler` + `DELETE /buildings/{buildingId}`
- [x] 2.2 `DeleteApartmentCommandHandler` + `DELETE /apartments/{apartmentId}`
- [x] 2.3 Расширить `ListApartmentsWithOwnersQueryHandler`: `lastReadingSubmittedAt`, `lastReadingValue`, `currentPeriodSubmitted`
- [x] 2.4 Unit-тесты handlers delete и расширенного list apartments

## 3. Backend — meter-readings

- [x] 3.1 Миграция: колонка `SubmittedAt` в `meter_readings`
- [x] 3.2 Обновить `SubmitMeterReadingCommandHandler` — запись `SubmittedAt`
- [x] 3.3 `ListApartmentMeterReadingsQueryHandler` + `GET /apartments/{apartmentId}/meter-readings`
- [x] 3.4 Unit-tесты list apartment readings и submittedAt

## 4. Frontend — API & shared

- [x] 4.1 `npm run generate:api` в `cnt_gq_web`
- [x] 4.2 Entity API: delete building/apartment, list apartment readings
- [x] 4.3 `shared/api/errors.ts` и `notifyApiError` (если ещё нет)

## 5. Frontend — features (модалки)

- [x] 5.1 `features/create-building` — `CreateBuildingDialog`
- [x] 5.2 `features/update-building` — `UpdateBuildingDialog`
- [x] 5.3 `features/delete-building` — confirm + API
- [x] 5.4 `features/create-apartment` — `CreateApartmentDialog`
- [x] 5.5 `features/delete-apartment` — confirm + API
- [x] 5.6 `features/upsert-apartment-owner` — `UpsertOwnerDialog`
- [x] 5.7 `features/submit-meter-reading` — `SubmitMeterReadingDialog`

## 6. Frontend — pages

- [x] 6.1 `pages/buildings` — таблица домов, кнопка «Добавить дом», row-click → apartments
- [x] 6.2 `pages/apartments` — таблица квартир (id, номер, владелец, дата, показание, статус), row-click → readings
- [x] 6.3 `pages/apartment-meter-readings` — таблица истории, кнопка «Передать показание»
- [x] 6.4 Удалить или заменить `pages/directories`, `pages/meter-readings`

## 7. Frontend — routing & layout

- [x] 7.1 Nested routes: `/`, `/buildings/:buildingId/apartments`, `/apartments/:apartmentId/meter-readings`
- [x] 7.2 Хлебные крошки в `AppLayout` или widget `Breadcrumbs`
- [x] 7.3 Убрать пункты меню «Справочники» / «Показания»; единая точка входа «Дома» или только breadcrumbs
- [x] 7.4 Опциональный редирект `/directories` → `/`

## 8. Frontend — FSD cleanup

- [x] 8.1 Composables `useBuildings`, `useApartments`, `useApartmentMeterReadings`
- [x] 8.2 Типы entities из OpenAPI; public API слайсов
- [x] 8.3 Presentation tables в entities (без сценарной логики в entity UI)
- [x] 8.4 ESLint boundaries (при наличии в проекте) — не настроено, базовый ESLint без boundaries
- [x] 8.5 `src/README.md` — карта слоёв

## 9. Docs & manifest

- [x] 9.1 `docs/architecture/specs/frontend.md` — описание новой навигации
- [x] 9.2 `manifest.yaml` — capability `frontend-fsd`

## 10. Verification

- [x] 10.1 Unit-тесты backend handlers
- [x] 10.2 `npm run lint && npm run build` в `cnt_gq_web`
- [x] 10.3 `.\scripts\verify.ps1`
- [x] 10.4 `npx openspec validate refactoring-frontend --strict --no-interactive`
