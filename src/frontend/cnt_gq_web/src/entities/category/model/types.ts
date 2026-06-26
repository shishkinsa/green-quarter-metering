/** DTO справочной категории из API. См. также `npm run generate:api` → shared/api/generated/openapi.d.ts */
export type Category = {
  id: string;
  code: string;
  name: string;
};

export type ListCategoriesResponse = {
  items: Category[];
};
