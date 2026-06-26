import { ElMessage } from 'element-plus';
import { ApiError } from '@/shared/api/errors';

/** Показывает пользователю сообщение об ошибке API. */
export function notifyApiError(error: unknown): void {
  if (error instanceof ApiError) {
    ElMessage.error(error.message);
    return;
  }

  if (error instanceof Error) {
    ElMessage.error(error.message);
    return;
  }

  ElMessage.error('Произошла ошибка');
}
