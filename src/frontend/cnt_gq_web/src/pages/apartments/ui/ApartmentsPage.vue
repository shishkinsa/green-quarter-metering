<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import {
  ApartmentsTable,
  createApartment,
  fetchBuildings,
  upsertApartmentOwner,
  useApartments,
  type ApartmentWithOwner,
  type Building,
} from '@/entities/building';
import { CreateApartmentDialog } from '@/features/create-apartment';
import { UpsertOwnerDialog } from '@/features/upsert-apartment-owner';
import { confirmDeleteApartment } from '@/features/delete-apartment';
import { notifyApiError } from '@/shared/lib/notifyApiError';

const route = useRoute();
const router = useRouter();

const buildingId = computed(() => route.params.buildingId as string | undefined);
const buildingName = computed(() => (route.query.buildingName as string) || building.value?.name || '');
const building = ref<Building | null>(null);
const { items, loading, load } = useApartments(buildingId);

const createVisible = ref(false);
const ownerVisible = ref(false);
const editingApartment = ref<ApartmentWithOwner | null>(null);

watch(buildingId, async (id) => {
  if (!id) return;
  const response = await fetchBuildings();
  building.value = response.items.find((b) => b.id === id) ?? null;
}, { immediate: true });

function goToReadings(apartment: ApartmentWithOwner) {
  void router.push({
    name: 'apartment-meter-readings',
    params: { apartmentId: apartment.id },
    query: {
      buildingId: apartment.buildingId,
      buildingName: buildingName.value || building.value?.name,
      apartmentNumber: apartment.number,
    },
  });
}

function openOwner(apartment: ApartmentWithOwner) {
  editingApartment.value = apartment;
  ownerVisible.value = true;
}

async function handleCreate(payload: { number: string; floor: number | null }) {
  if (!buildingId.value) return;
  try {
    await createApartment(buildingId.value, payload.number, payload.floor);
    ElMessage.success('Квартира добавлена');
    createVisible.value = false;
    await load();
  } catch (error) {
    notifyApiError(error);
  }
}

async function handleSaveOwner(payload: { apartmentId: string; fullName: string; phone: string | null }) {
  try {
    await upsertApartmentOwner(payload.apartmentId, payload.fullName, payload.phone);
    ElMessage.success('Владелец сохранён');
    ownerVisible.value = false;
    await load();
  } catch (error) {
    notifyApiError(error);
  }
}

async function handleDelete(apartment: ApartmentWithOwner) {
  try {
    await confirmDeleteApartment(apartment);
    ElMessage.success('Квартира удалена');
    await load();
  } catch (error) {
    if (error !== 'cancel') {
      notifyApiError(error);
    }
  }
}
</script>

<template>
  <div>
    <el-page-header
      :content="buildingName ? `Квартиры — ${buildingName}` : 'Квартиры'"
      @back="router.push('/')"
    />

    <div style="margin: 16px 0">
      <el-button type="primary" @click="createVisible = true">Добавить квартиру</el-button>
    </div>

    <ApartmentsTable
      :items="items"
      :loading="loading"
      @row-click="goToReadings"
      @edit-owner="openOwner"
      @delete="handleDelete"
    />

    <CreateApartmentDialog v-model:visible="createVisible" @submit="handleCreate" />
    <UpsertOwnerDialog
      v-model:visible="ownerVisible"
      :apartment="editingApartment"
      @submit="handleSaveOwner"
    />
  </div>
</template>
