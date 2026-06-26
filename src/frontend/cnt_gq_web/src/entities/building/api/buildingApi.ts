import { apiFetch } from '@/shared/api/http';
import type {
  CreateApartmentResponse,
  CreateBuildingResponse,
  ListApartmentsWithOwnersResponse,
  ListBuildingsResponse,
  UpsertApartmentOwnerResponse,
} from '@/entities/building/model/types';

/**
 * Возвращает список домов.
 */
export function fetchBuildings(): Promise<ListBuildingsResponse> {
  return apiFetch<ListBuildingsResponse>('/v1/buildings');
}

/**
 * Возвращает квартиры дома с данными владельцев.
 *
 * @param buildingId — идентификатор дома
 */
export function fetchApartmentsWithOwners(buildingId: string): Promise<ListApartmentsWithOwnersResponse> {
  return apiFetch<ListApartmentsWithOwnersResponse>(`/v1/buildings/${buildingId}/apartments`);
}

/**
 * Создаёт дом.
 */
export function createBuilding(name: string, address: string | null): Promise<CreateBuildingResponse> {
  return apiFetch<CreateBuildingResponse>('/v1/buildings', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, address }),
  });
}

/**
 * Обновляет дом.
 */
export function updateBuilding(id: string, name: string, address: string | null): Promise<CreateBuildingResponse> {
  return apiFetch<CreateBuildingResponse>(`/v1/buildings/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, address }),
  });
}

/**
 * Создаёт квартиру в доме.
 */
export function createApartment(
  buildingId: string,
  number: string,
  floor: number | null,
): Promise<CreateApartmentResponse> {
  return apiFetch<CreateApartmentResponse>(`/v1/buildings/${buildingId}/apartments`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ number, floor }),
  });
}

/**
 * Назначает или обновляет владельца квартиры.
 */
export function upsertApartmentOwner(
  apartmentId: string,
  fullName: string,
  phone: string | null,
): Promise<UpsertApartmentOwnerResponse> {
  return apiFetch<UpsertApartmentOwnerResponse>(`/v1/apartments/${apartmentId}/owner`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ fullName, phone }),
  });
}
