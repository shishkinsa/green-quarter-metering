import { ref, watch, type Ref } from 'vue';
import { fetchApartmentsWithOwners } from '@/entities/building/api/buildingApi';
import type { ApartmentWithOwner } from '@/entities/building/model/types';

/** Composable загрузки квартир дома. */
export function useApartments(buildingId: Ref<string | undefined>) {
  const items = ref<ApartmentWithOwner[]>([]);
  const loading = ref(false);

  async function load() {
    if (!buildingId.value) {
      items.value = [];
      return;
    }

    loading.value = true;
    try {
      const response = await fetchApartmentsWithOwners(buildingId.value);
      items.value = response.items;
    } finally {
      loading.value = false;
    }
  }

  watch(buildingId, () => {
    void load();
  }, { immediate: true });

  return { items, loading, load };
}
