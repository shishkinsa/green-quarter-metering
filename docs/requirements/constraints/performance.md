---
id: NFR-001
type: constraint
status: draft
---

# Производительность API

Для эталонных эндпоинтов `/api/v1/health` и `/api/v1/examples` при нагрузке из [capacity.md](../../architecture/planning/capacity.md) время ответа p95 не превышает целевого значения.

| Метрика | Цель (черновик) | Условия |
|---------|-----------------|---------|
| p95 latency GET /health | < 50 ms | 1 инстанс, без БД |
| p95 latency GET /examples | < 200 ms | до 1000 записей в `example_items` |

TBD: уточнить после заполнения расчёта нагрузки.
