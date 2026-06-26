# ADR-0005: Requestum для CQRS

## 📋 Метаданные

| Атрибут | Значение |
|---------|----------|
| **Статус** | Принято |
| **Дата** | 2026-06-23 |
| **Автор** | Зеленый квартал — Учёт электроэнергии Team |
| **Версия** | 1.0 |

## 🎯 Контекст

UseCases backend должны разделять команды и запросы без раздувания контроллеров.

## ✅ Решение

Использовать **Requestum** (`ICommand`, `IQuery`, handlers) в слое `3 UseCases`.

## 📎 Последствия

- Handlers в `Handlers/{Entity}/Commands|Queries`
- Контроллеры вызывают `IRequestum.ExecuteAsync` / `HandleAsync`
- Регистрация: `AddRequestum` в `Program.cs`
