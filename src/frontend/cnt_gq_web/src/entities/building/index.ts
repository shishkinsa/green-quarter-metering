export type {
  ApartmentWithOwner,
  Building,
  Owner,
} from './model/types';
export {
  createApartment,
  createBuilding,
  deleteApartment,
  deleteBuilding,
  fetchApartmentsWithOwners,
  fetchBuildings,
  updateBuilding,
  upsertApartmentOwner,
} from './api/buildingApi';
export { useBuildings } from './model/useBuildings';
export { useApartments } from './model/useApartments';
export { default as ApartmentsTable } from './ui/ApartmentsTable.vue';
export { default as BuildingsTable } from './ui/BuildingsTable.vue';
