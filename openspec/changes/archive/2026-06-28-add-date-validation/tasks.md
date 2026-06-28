## 1. Спецификация и контракт

- [x] 1.1 Проверить delta specs: `npx openspec validate add-date-validation --strict --no-interactive`
- [x] 1.2 OpenAPI: `meterVerificationDate` в `CreateApartmentRequest`, `UpdateApartmentRequest`, `ApartmentWithOwnerDto`; PUT `/apartments/{apartmentId}`

## 2. Backend — домен и API

- [x] 2.1 Добавить `MeterVerificationDate` (`DateOnly?`) в entity `Apartment`; методы create/update с валидацией даты
- [x] 2.2 EF-конфигурация и миграция `AddApartmentMeterVerificationDate`
- [x] 2.3 Расширить `CreateApartmentCommand` полем `DateOnly? MeterVerificationDate`; validator — дата не в будущем
- [x] 2.4 Реализовать `UpdateApartmentCommand` + handler + validator + PUT в controller
- [x] 2.5 `ApartmentQueries` и DTO/mappings: `meterVerificationDate` в списке и ответах create/update
- [x] 2.6 Unit-тесты create/update: сохранение даты, будущая дата → 400, очистка даты, конфликт номера

## 3. Frontend

- [x] 3.1 Обновить типы и API: `createApartment`, новый `updateApartment`
- [x] 3.2 Модалка квартиры (create/edit): поля номер, этаж, date picker «Дата поверки счётчика»
- [x] 3.3 `ApartmentsTable`: колонка «Дата поверки»; кнопка «Редактировать» → модалка квартиры
- [x] 3.4 `ApartmentsPage`: обработчики create и update

## 4. Проверка

- [x] 4.1 `.\scripts\verify.ps1`
- [x] 4.2 `npx openspec validate add-date-validation --strict --no-interactive`
