const AUTH_ENABLED = import.meta.env.VITE_AUTH_ENABLED === 'true';
const TOKEN_STORAGE_KEY = 'sp.auth.token';
const DEV_TOKEN = 'dev-token';

/**
 * Включена ли аутентификация на клиенте (синхронно с Auth:Enabled на API).
 */
export function isAuthEnabled(): boolean {
  return AUTH_ENABLED;
}

/**
 * Возвращает Bearer-токен для API или null.
 */
export function getAuthToken(): string | null {
  if (!AUTH_ENABLED) {
    return null;
  }

  return sessionStorage.getItem(TOKEN_STORAGE_KEY) ?? DEV_TOKEN;
}

/**
 * Сохраняет токен в sessionStorage (заглушка до интеграции OIDC).
 */
export function setAuthToken(token: string): void {
  sessionStorage.setItem(TOKEN_STORAGE_KEY, token);
}
