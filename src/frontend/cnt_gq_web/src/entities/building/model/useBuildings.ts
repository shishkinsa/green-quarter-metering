import { onMounted, ref } from 'vue';
import { fetchBuildings } from '@/entities/building/api/buildingApi';
import type { Building } from '@/entities/building/model/types';

/** Composable загрузки списка домов. */
export function useBuildings() {
  const items = ref<Building[]>([]);
  const loading = ref(false);

  async function load() {
    loading.value = true;
    try {
      const response = await fetchBuildings();
      items.value = response.items;
    } finally {
      loading.value = false;
    }
  }

  onMounted(() => {
    void load();
  });

  return { items, loading, load };
}
