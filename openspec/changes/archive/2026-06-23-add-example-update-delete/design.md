## Context

Эталонная сущность `ExampleItem` уже реализована (create/list/get). Pilot дополняет CRUD без изменения схемы БД.

## Goals / Non-Goals

- Goals: PUT/DELETE API, UI edit/delete в таблице, тесты, archive change
- Non-Goals: soft delete, pagination, optimistic locking

## Decisions

- **Update**: `ExampleItem.Rename(name)` на домене; PUT возвращает 200 + updated item
- **Delete**: hard delete через EF; DELETE возвращает 204 No Content
- **404**: `UseCaseNotFoundException` для update/delete несуществующего id (как get)
- **Frontend**: `features/example/edit-item` (Modal), `features/example/delete-item` (Popconfirm); actions-колонка в `ExampleTable`

## Risks / Trade-offs

- [Concurrent edit] → вне scope pilot; last-write-wins

## Migration Plan

Deploy backend + frontend together. Rollback: revert endpoints; orphaned data не возникает.
