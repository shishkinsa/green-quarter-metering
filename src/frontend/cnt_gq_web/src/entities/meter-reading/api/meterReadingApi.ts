import { apiFetch } from '@/shared/api/http';
import type {
  ListBuildingMeterReadingsResponse,
  SubmitMeterReadingPayload,
  SubmitMeterReadingResponse,
} from '@/entities/meter-reading/model/types';

/**
 * Передаёт или обновляет показание по квартире.
 *
 * @param apartmentId — идентификатор квартиры
 * @param payload — отчётный период и значение счётчика
 */
export function submitMeterReading(
  apartmentId: string,
  payload: SubmitMeterReadingPayload,
): Promise<SubmitMeterReadingResponse> {
  return apiFetch<SubmitMeterReadingResponse>(`/v1/apartments/${apartmentId}/meter-readings`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
}

/**
 * Возвращает сводку сдачи показаний по дому за отчётный период.
 *
 * @param buildingId — идентификатор дома
 * @param periodYear — год отчётного периода
 * @param periodMonth — месяц отчётного периода (1–12)
 */
export function listBuildingMeterReadings(
  buildingId: string,
  periodYear: number,
  periodMonth: number,
): Promise<ListBuildingMeterReadingsResponse> {
  const params = new URLSearchParams({
    periodYear: String(periodYear),
    periodMonth: String(periodMonth),
  });
  return apiFetch<ListBuildingMeterReadingsResponse>(
    `/v1/buildings/${buildingId}/meter-readings?${params.toString()}`,
  );
}
