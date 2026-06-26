/** DTO примера сущности из API. См. также `npm run generate:api` → shared/api/generated/openapi.d.ts */
export type ExampleItem = {
  id: string;
  name: string;
  createdAt: string;
};

export type ListExamplesResponse = {
  items: ExampleItem[];
};

export type CreateExampleResponse = {
  item: ExampleItem;
};

export type UpdateExampleResponse = {
  item: ExampleItem;
};
