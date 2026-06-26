import { parseApiError } from './errors';

/**
 * Базовый HTTP-клиент для вызовов `/api`.
 *
 * @param path — путь относительно `/api`
 * @param init — параметры fetch
 * @returns распарсенный JSON
 * @throws {ApiError} при статусе ответа не 2xx
 */
export async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  const headers = new Headers(init?.headers);
  headers.set('Accept', 'application/json');

  const response = await fetch(`/api${path}`, {
    ...init,
    headers,
  });

  if (!response.ok) {
    throw await parseApiError(response);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}
