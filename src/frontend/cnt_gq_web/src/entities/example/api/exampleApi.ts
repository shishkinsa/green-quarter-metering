import { apiFetch } from '@/shared/api/http';
import type {
  CreateExampleResponse,
  ListExamplesResponse,
  UpdateExampleResponse,
} from '@/entities/example/model/types';

/**
 * Загружает список примеров сущностей.
 */
export function fetchExamples(): Promise<ListExamplesResponse> {
  return apiFetch<ListExamplesResponse>('/v1/examples');
}

/**
 * Создаёт пример сущности.
 *
 * @param name — наименование
 */
export function createExample(name: string): Promise<CreateExampleResponse> {
  return apiFetch<CreateExampleResponse>('/v1/examples', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  });
}

/**
 * Обновляет пример сущности.
 *
 * @param id — идентификатор
 * @param name — новое наименование
 */
export function updateExample(id: string, name: string): Promise<UpdateExampleResponse> {
  return apiFetch<UpdateExampleResponse>(`/v1/examples/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  });
}

/**
 * Удаляет пример сущности.
 *
 * @param id — идентификатор
 */
export function deleteExample(id: string): Promise<void> {
  return apiFetch<void>(`/v1/examples/${id}`, {
    method: 'DELETE',
  });
}
