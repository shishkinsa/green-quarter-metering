export interface Building {
  id: string;
  name: string;
  address: string | null;
}

export interface ApartmentWithOwner {
  id: string;
  buildingId: string;
  number: string;
  floor: number | null;
  ownerId: string | null;
  ownerFullName: string | null;
  ownerPhone: string | null;
}

export interface Owner {
  id: string;
  apartmentId: string;
  fullName: string;
  phone: string | null;
}

export interface ListBuildingsResponse {
  items: Building[];
}

export interface ListApartmentsWithOwnersResponse {
  items: ApartmentWithOwner[];
}

export interface CreateBuildingResponse {
  item: Building;
}

export interface CreateApartmentResponse {
  item: ApartmentWithOwner;
}

export interface UpsertApartmentOwnerResponse {
  item: Owner;
}
