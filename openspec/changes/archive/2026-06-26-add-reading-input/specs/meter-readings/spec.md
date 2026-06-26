---
capability: meter-readings
business: docs/requirements/business/capabilities.md#meter-readings
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## Purpose

Передача и учёт показаний электросчётчиков по квартирам ЖК «Зелёный квартал». Система SHALL сохранять показания с привязкой к отчётному месяцу, проверять монотонность значений и предоставлять энергетику сводку сдачи по дому.

REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../../../docs/architecture/openapi/components/openapi.yaml)

## ADDED Requirements

### Requirement: Передача показания по квартире

Система SHALL принимать текущее показание электросчётчика для квартиры и отчётного периода (календарный год и месяц).

#### Scenario: Успешная передача нового показания

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с телом `{"periodYear":2026,"periodMonth":6,"value":12345.5}`
- **AND** квартира существует
- **AND** для квартиры нет показания за указанный период
- **AND** значение `value` не меньше последнего сохранённого показания по этой квартире (если предыдущее показание есть)
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `apartmentId`, `periodYear`, `periodMonth`, `value`

#### Scenario: Обновление показания за тот же месяц

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с телом `{"periodYear":2026,"periodMonth":6,"value":12350}`
- **AND** для квартиры уже есть показание за период 2026-06
- **AND** новое `value` не меньше последнего показания за предыдущие периоды
- **THEN** статус ответа 200
- **AND** тело ответа `item` содержит обновлённое `value` для того же периода

#### Scenario: Показание меньше предыдущего

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings
- **AND** переданное `value` меньше последнего сохранённого показания по квартире (за любой предшествующий период)
- **THEN** статус ответа 400
- **AND** тело ответа содержит описание ошибки валидации

#### Scenario: Ошибка валидации периода

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings с `periodMonth` вне диапазона 1–12 или с отрицательным `value`
- **THEN** статус ответа 400

#### Scenario: Квартира не найдена

- **WHEN** клиент отправляет POST /api/v1/apartments/{apartmentId}/meter-readings для несуществующей квартиры
- **THEN** статус ответа 404

### Requirement: Сводка сдачи показаний по дому

Система SHALL возвращать по выбранному дому и отчётному месяцу список квартир с признаком сдачи показания.

#### Scenario: Успешное получение сводки

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/meter-readings?periodYear=2026&periodMonth=6
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items`
- **AND** каждый элемент содержит: `apartmentId`, `apartmentNumber`, `ownerFullName`, `readingValue` (число или `null`, если не сдано), `submitted` (boolean)
- **AND** элементы отсортированы по `apartmentNumber` по возрастанию

#### Scenario: Дом не найден при получении сводки

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/meter-readings для несуществующего дома
- **THEN** статус ответа 404

#### Scenario: Ошибка валидации параметров периода

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/meter-readings без `periodYear`/`periodMonth` или с `periodMonth` вне 1–12
- **THEN** статус ответа 400

### Requirement: Хранение показаний в базе данных

Система SHALL персистировать показания в PostgreSQL с уникальностью по квартире и отчётному периоду.

#### Scenario: Структура таблицы показаний

- **WHEN** применена миграция capability `meter-readings`
- **THEN** в БД существует таблица `meter_readings` с колонками `Id` (uuid, PK), `ApartmentId` (uuid, FK → `apartments`), `PeriodYear` (int, NOT NULL), `PeriodMonth` (int, NOT NULL), `Value` (numeric, NOT NULL)
- **AND** уникальное ограничение на тройку (`ApartmentId`, `PeriodYear`, `PeriodMonth`)

### Requirement: Форма передачи показания в SPA

SPA SHALL предоставлять форму ввода показания с выбором квартиры, отчётного месяца и значения счётчика.

#### Scenario: Успешная отправка из формы

- **WHEN** пользователь выбирает дом и квартиру, указывает год, месяц и значение показания и подтверждает отправку
- **AND** API возвращает успешный ответ
- **THEN** отображается подтверждение успешной передачи

#### Scenario: Отображение ошибки валидации в форме

- **WHEN** API возвращает 400 (показание меньше предыдущего или невалидный период)
- **THEN** форма показывает сообщение об ошибке без потери введённых данных

### Requirement: Таблица статуса сдачи в SPA

SPA SHALL отображать сводку сдачи показаний по выбранному дому и отчётному месяцу.

#### Scenario: Просмотр статуса сдачи

- **WHEN** пользователь выбирает дом, год и месяц на странице показаний
- **THEN** отображается таблица с колонками: «Номер квартиры», «ФИО владельца», «Показание», «Сдано»
- **AND** данные загружаются из GET /api/v1/buildings/{buildingId}/meter-readings

#### Scenario: Квартира без показания

- **WHEN** для квартиры нет показания за выбранный период
- **THEN** в колонке «Показание» отображается прочерк
- **AND** в колонке «Сдано» отображается признак «нет»
