import type { ListCategoriesResponse } from '@/entities/category/model/types';
import { apiFetch } from '@/shared/api/http';

/**
 * Возвращает список справочных категорий.
 */
export async function fetchCategories(): Promise<ListCategoriesResponse> {
  return apiFetch<ListCategoriesResponse>('/v1/categories');
}
