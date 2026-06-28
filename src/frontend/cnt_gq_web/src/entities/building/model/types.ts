import type { components } from '@/shared/api/generated/openapi';

export type Building = components['schemas']['BuildingDto'];
export type ApartmentWithOwner = components['schemas']['ApartmentWithOwnerDto'];
export type Owner = components['schemas']['OwnerDto'];

export type ListBuildingsResponse = components['schemas']['ListBuildingsResponse'];
export type ListApartmentsWithOwnersResponse = components['schemas']['ListApartmentsWithOwnersResponse'];
export type CreateBuildingResponse = components['schemas']['CreateBuildingResponse'];
export type UpdateBuildingResponse = components['schemas']['UpdateBuildingResponse'];
export type CreateApartmentResponse = components['schemas']['CreateApartmentResponse'];
export type UpdateApartmentResponse = components['schemas']['UpdateApartmentResponse'];
export type UpsertApartmentOwnerResponse = components['schemas']['UpsertApartmentOwnerResponse'];
