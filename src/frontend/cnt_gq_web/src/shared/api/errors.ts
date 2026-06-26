/** Ошибка HTTP API с телом ProblemDetails. */
export class ApiError extends Error {
  readonly status: number;
  readonly title: string | null;
  readonly detail: string | null;

  constructor(status: number, title: string | null, detail: string | null) {
    super(detail ?? title ?? `API error: ${status}`);
    this.name = 'ApiError';
    this.status = status;
    this.title = title;
    this.detail = detail;
  }
}

interface ProblemDetails {
  title?: string;
  detail?: string;
}

/** Парсит тело ошибки API в {@link ApiError}. */
export async function parseApiError(response: Response): Promise<ApiError> {
  let title: string | null = null;
  let detail: string | null = null;

  try {
    const body = (await response.json()) as ProblemDetails;
    title = body.title ?? null;
    detail = body.detail ?? null;
  } catch {
    // ignore non-JSON body
  }

  return new ApiError(response.status, title, detail);
}
