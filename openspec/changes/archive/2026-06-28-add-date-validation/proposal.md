## Why

Энергетику нужно учитывать срок поверки электросчётчика по каждой квартире. Сейчас в справочнике нет поля даты поверки; при создании и редактировании квартиры можно указать только номер и этаж.

## What Changes

- Добавить поле **дата поверки счётчика** (`meterVerificationDate`) на уровне квартиры (календарная дата, nullable).
- Расширить **POST** `/api/v1/buildings/{buildingId}/apartments`: опциональная дата поверки при создании квартиры.
- Добавить **PUT** `/api/v1/apartments/{apartmentId}`: обновление номера, этажа и даты поверки (реализация сценария «изменение квартиры» из spec `directories`).
- Валидация даты: не позже текущего календарного дня (UTC); формат ISO 8601 date (`YYYY-MM-DD`).
- Возвращать `meterVerificationDate` в ответах create/update и в списке квартир (`ApartmentWithOwnerDto`).
- Frontend: поле «Дата поверки» в модалках **добавления и редактирования** квартиры; колонка в таблице.
- Миграция EF: колонка `MeterVerificationDate` (`date`, nullable) в `apartments`.

## Capabilities

### New Capabilities

_(нет)_

### Modified Capabilities

- `directories`: хранение, API create/update квартиры, валидация даты поверки.
- `frontend-fsd`: формы и таблица квартир с датой поверки.

## Impact

- **Spec:** delta `directories`, `frontend-fsd`.
- **OpenAPI:** `CreateApartmentRequest`, новый `UpdateApartmentRequest`, `ApartmentWithOwnerDto`; PUT `/apartments/{apartmentId}`.
- **Backend:** `Apartment`, `CreateApartmentCommand`, новый `UpdateApartmentCommand`, validators, handlers, `ApartmentQueries`, миграция.
- **Frontend:** `CreateApartmentDialog` → upsert-диалог квартиры, новый `update-apartment` feature (или расширение create), `ApartmentsTable`.
- **БД:** миграция `apartments.MeterVerificationDate`.
