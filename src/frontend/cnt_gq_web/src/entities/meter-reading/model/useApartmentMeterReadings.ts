import { ref, watch, type Ref } from 'vue';
import { listApartmentMeterReadings } from '@/entities/meter-reading/api/meterReadingApi';
import type { MeterReading } from '@/entities/meter-reading/model/types';

/** Composable загрузки истории показаний квартиры. */
export function useApartmentMeterReadings(apartmentId: Ref<string | undefined>) {
  const items = ref<MeterReading[]>([]);
  const loading = ref(false);

  async function load() {
    if (!apartmentId.value) {
      items.value = [];
      return;
    }

    loading.value = true;
    try {
      const response = await listApartmentMeterReadings(apartmentId.value);
      items.value = response.items;
    } finally {
      loading.value = false;
    }
  }

  watch(apartmentId, () => {
    void load();
  }, { immediate: true });

  return { items, loading, load };
}
