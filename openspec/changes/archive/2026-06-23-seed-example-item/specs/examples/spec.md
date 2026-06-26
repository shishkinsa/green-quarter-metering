# Delta for Examples (seed)

## ADDED Requirements

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
- **AND** the response body contains `id` and `name`

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
