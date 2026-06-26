## 1. Spec & Contract

- [x] 1.1 Delta specs validated
- [x] 1.2 OpenAPI: PUT/DELETE `/examples/{id}`
- [x] 1.3 Типы id без изменений (UUID)

## 2. Backend (Clean Architecture)

- [x] 2.1 `ExampleItem.Rename(name)` на домене
- [x] 2.2 `IExampleItemRepository`: UpdateAsync, DeleteAsync
- [x] 2.3 `UpdateExampleCommand`, `DeleteExampleCommand` + Handlers + Validators
- [x] 2.4 `ExamplesController`: PUT, DELETE
- [x] 2.5 Unit + integration tests

## 3. Frontend (FSD)

- [x] 3.1 `entities/example/api`: updateExample, deleteExample
- [x] 3.2 `features/example/edit-item`: EditExampleModal
- [x] 3.3 `features/example/delete-item`: DeleteExampleButton
- [x] 3.4 `ExampleTable` — колонка действий
- [x] 3.5 `HomePage` — wiring

## 4. Docs & Architecture

- [x] 4.1 Матрица трассировки обновлена
- [x] 4.2 Workflow doc: docs/process/workflows/change-lifecycle.md

## 5. Verification

- [x] 5.1 `.\scripts\verify.ps1`
- [x] 5.2 Archive change → merge specs
