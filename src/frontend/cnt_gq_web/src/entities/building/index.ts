export type {
  ApartmentWithOwner,
  Building,
  Owner,
} from './model/types';
export {
  createApartment,
  createBuilding,
  fetchApartmentsWithOwners,
  fetchBuildings,
  updateBuilding,
  upsertApartmentOwner,
} from './api/buildingApi';
export { default as ApartmentsTable } from './ui/ApartmentsTable.vue';
