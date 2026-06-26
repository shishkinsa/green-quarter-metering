import type { components } from '@/shared/api/generated/openapi';

export type MeterReading = components['schemas']['MeterReadingDto'];
export type BuildingMeterReadingStatus = components['schemas']['BuildingMeterReadingStatusDto'];

export type SubmitMeterReadingResponse = components['schemas']['SubmitMeterReadingResponse'];
export type ListBuildingMeterReadingsResponse = components['schemas']['ListBuildingMeterReadingsResponse'];
export type ListApartmentMeterReadingsResponse = components['schemas']['ListApartmentMeterReadingsResponse'];

export type SubmitMeterReadingPayload = components['schemas']['SubmitMeterReadingRequest'];
