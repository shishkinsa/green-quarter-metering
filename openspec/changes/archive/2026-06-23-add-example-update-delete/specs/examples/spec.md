## ADDED Requirements

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
