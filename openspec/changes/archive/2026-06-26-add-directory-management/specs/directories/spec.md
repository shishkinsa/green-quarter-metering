---
capability: directories
business: docs/requirements/business/capabilities.md#directories
openapi: docs/architecture/openapi/components/openapi.yaml
adr: []
---

## Purpose

Справочник объектов ЖК «Зелёный квартал»: дома, квартиры и владельцы. Система SHALL хранить сущности в PostgreSQL и предоставлять REST API и табличное отображение квартир выбранного дома вместе с данными владельцев.

REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../../../docs/architecture/openapi/components/openapi.yaml)

## ADDED Requirements

### Requirement: Список домов

Система SHALL возвращать список домов (корпусов) ЖК.

#### Scenario: Успешное получение списка домов

- **WHEN** клиент отправляет GET /api/v1/buildings
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items` с объектами дома (`id`, `name`, `address`)

#### Scenario: Сортировка домов по наименованию

- **WHEN** клиент отправляет GET /api/v1/buildings
- **THEN** элементы `items` отсортированы по полю `name` по возрастанию

### Requirement: Список квартир дома с владельцами

Система SHALL возвращать квартиры выбранного дома вместе с данными владельца каждой квартиры — для отображения в таблице.

#### Scenario: Успешное получение таблицы квартир и владельцев

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments
- **THEN** статус ответа 200
- **AND** тело ответа содержит массив `items`
- **AND** каждый элемент содержит поля квартиры: `id`, `buildingId`, `number`, `floor`
- **AND** каждый элемент содержит поля владельца: `ownerId`, `ownerFullName`, `ownerPhone` (допускается `null`, если владелец не назначен)

#### Scenario: Сортировка квартир по номеру

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments
- **THEN** элементы `items` отсортированы по полю `number` по возрастанию

#### Scenario: Дом не найден

- **WHEN** клиент отправляет GET /api/v1/buildings/{buildingId}/apartments для несуществующего `buildingId`
- **THEN** статус ответа 404

### Requirement: Создание и изменение дома

Система SHALL позволять создавать и обновлять записи домов.

#### Scenario: Успешное создание дома

- **WHEN** клиент отправляет POST /api/v1/buildings с телом `{"name":"Корпус 1","address":"ул. Зелёная, 1"}`
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `name`, `address`

#### Scenario: Ошибка валидации при создании дома

- **WHEN** клиент отправляет POST /api/v1/buildings с пустым `name`
- **THEN** статус ответа 400

#### Scenario: Успешное обновление дома

- **WHEN** клиент отправляет PUT /api/v1/buildings/{id} с валидными `name` и `address`
- **THEN** статус ответа 200
- **AND** тело ответа `item` отражает обновлённые значения

### Requirement: Создание и изменение квартиры

Система SHALL позволять создавать и обновлять квартиры в рамках существующего дома.

#### Scenario: Успешное создание квартиры

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments с телом `{"number":"12","floor":3}`
- **THEN** статус ответа 201
- **AND** тело ответа `item` содержит `id`, `buildingId`, `number`, `floor`

#### Scenario: Дубликат номера квартиры в доме

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments с `number`, уже существующим в этом доме
- **THEN** статус ответа 409

#### Scenario: Дом не найден при создании квартиры

- **WHEN** клиент отправляет POST /api/v1/buildings/{buildingId}/apartments для несуществующего дома
- **THEN** статус ответа 404

### Requirement: Назначение и изменение владельца квартиры

Система SHALL хранить владельца как отдельную сущность, связанную с квартирой (связь один-к-одному: одна квартира — один владелец).

#### Scenario: Успешное назначение владельца

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner с телом `{"fullName":"Иванов Иван Иванович","phone":"+79001234567"}`
- **THEN** статус ответа 200
- **AND** тело ответа `item` содержит `id`, `apartmentId`, `fullName`, `phone`

#### Scenario: Ошибка валидации владельца

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner с пустым `fullName`
- **THEN** статус ответа 400

#### Scenario: Квартира не найдена

- **WHEN** клиент отправляет PUT /api/v1/apartments/{apartmentId}/owner для несуществующей квартиры
- **THEN** статус ответа 404

### Requirement: Хранение сущностей в базе данных

Система SHALL персистировать дома, квартиры и владельцев в PostgreSQL.

#### Scenario: Структура таблицы домов

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `buildings` с колонками `Id` (uuid, PK), `Name` (varchar, NOT NULL), `Address` (varchar, nullable)

#### Scenario: Структура таблицы квартир

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `apartments` с колонками `Id` (uuid, PK), `BuildingId` (uuid, FK → `buildings`), `Number` (varchar, NOT NULL), `Floor` (int, nullable)
- **AND** уникальное ограничение на пару (`BuildingId`, `Number`)

#### Scenario: Структура таблицы владельцев

- **WHEN** применена миграция capability `directories`
- **THEN** в БД существует таблица `owners` с колонками `Id` (uuid, PK), `ApartmentId` (uuid, FK → `apartments`, UNIQUE), `FullName` (varchar, NOT NULL), `Phone` (varchar, nullable)

### Requirement: Таблица квартир и владельцев в SPA

SPA SHALL отображать квартиры выбранного дома и владельцев в виде таблицы.

#### Scenario: Выбор дома на странице справочников

- **WHEN** пользователь открывает страницу справочников
- **THEN** отображается список домов из GET /api/v1/buildings

#### Scenario: Таблица квартир и владельцев

- **WHEN** пользователь выбирает дом на странице справочников
- **THEN** отображается таблица с колонками: «Номер квартиры», «Этаж», «ФИО владельца», «Телефон»
- **AND** данные загружаются из GET /api/v1/buildings/{buildingId}/apartments

#### Scenario: Пустой владелец в таблице

- **WHEN** у квартиры не назначен владелец
- **THEN** в таблице в колонках «ФИО владельца» и «Телефон» отображается прочерк или пустое значение
