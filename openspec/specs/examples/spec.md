---
capability: examples
business: docs/requirements/business/capabilities.md#examples
openapi: docs/architecture/openapi/components/openapi.yaml
adr: ["0005", "0007", "0008"]
---

## Purpose

REST API и SPA для демонстрации сквозного сценария шаблона: полный CRUD примеров сущностей (create / list / get / update / delete).

REST-контракт: [docs/architecture/openapi/components/openapi.yaml](../../../docs/architecture/openapi/components/openapi.yaml)

## Requirements

### Requirement: Health Check

The system SHALL expose a health endpoint for readiness checks.

#### Scenario: Health endpoint available

- **WHEN** a client sends GET /api/v1/health
- **THEN** the response status is 200

### Requirement: Create Example Item

The system SHALL allow clients to create example items via REST API.

#### Scenario: Successful creation

- **WHEN** a client sends POST /api/v1/examples with body `{"name":"Test"}`
- **THEN** the response status is 201
- **AND** the response body `item` contains `id` and `name`

#### Scenario: Validation error

- **WHEN** a client sends POST /api/v1/examples with an invalid or empty name
- **THEN** the response status is 400

### Requirement: List Example Items

The system SHALL return a list of all example items.

#### Scenario: List after creation

- **WHEN** a client sends GET /api/v1/examples after creating items
- **THEN** the response status is 200
- **AND** the response body contains an `items` array with created entities

### Requirement: Get Example Item By Id

The system SHALL return a single example item by UUID.

#### Scenario: Get existing item

- **WHEN** a client sends GET /api/v1/examples/{id} for an existing id
- **THEN** the response status is 200
- **AND** the response body contains the same `id` and `name` as created

#### Scenario: Item not found

- **WHEN** a client sends GET /api/v1/examples/{id} for a non-existent id
- **THEN** the response status is 404

### Requirement: Display Examples In SPA

The SPA SHALL display example items on the home page and allow creating new items via a form.

#### Scenario: View list on home page

- **WHEN** a user opens the home page
- **THEN** the page displays a table of example items from the API

#### Scenario: Create via form

- **WHEN** a user submits the create form with a valid name
- **THEN** the new item appears in the list without a full page reload

### Requirement: Update Example Item

The system SHALL allow clients to update an example item name via REST API.

#### Scenario: Successful update

- **WHEN** a client sends PUT /api/v1/examples/{id} with body `{"name":"Updated"}`
- **THEN** the response status is 200
- **AND** the response body contains the updated `name`

#### Scenario: Update validation error

- **WHEN** a client sends PUT /api/v1/examples/{id} with an invalid or empty name
- **THEN** the response status is 400

#### Scenario: Update not found

- **WHEN** a client sends PUT /api/v1/examples/{id} for a non-existent id
- **THEN** the response status is 404

### Requirement: Delete Example Item

The system SHALL allow clients to delete an example item via REST API.

#### Scenario: Successful delete

- **WHEN** a client sends DELETE /api/v1/examples/{id} for an existing id
- **THEN** the response status is 204
- **AND** subsequent GET /api/v1/examples/{id} returns 404

#### Scenario: Delete not found

- **WHEN** a client sends DELETE /api/v1/examples/{id} for a non-existent id
- **THEN** the response status is 404

### Requirement: Edit And Delete Examples In SPA

The SPA SHALL allow users to edit and delete example items from the home page table.

#### Scenario: Edit via modal

- **WHEN** a user clicks edit on a table row and submits a valid name
- **THEN** the row shows the updated name without a full page reload

#### Scenario: Delete via confirmation

- **WHEN** a user confirms delete on a table row
- **THEN** the row is removed from the list without a full page reload

