# ADR-0006: OpenTelemetry для observability

## 📋 Метаданные

| Атрибут | Значение |
|---------|----------|
| **Статус** | Принято |
| **Дата** | 2026-06-23 |
| **Автор** | Зеленый квартал — Учёт электроэнергии Team |
| **Версия** | 1.0 |

## 🎯 Контекст

Нужна единая трассировка, метрики и логи для локальной разработки и production.

## ✅ Решение

Использовать **OpenTelemetry** в backend (`GQ.Shared.Observability`) с экспортом OTLP в Collector → Prometheus / Loki / Tempo / Grafana.

## 📎 Последствия

- Конфигурация: `monitoring/`, `docker-compose.monitoring.yml`
- Переменная `OTEL_EXPORTER_OTLP_ENDPOINT` для backend
- Grafana dashboards в `monitoring/grafana/provisioning/dashboards/`
