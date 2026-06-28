<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import {
  ApartmentsTable,
  createApartment,
  fetchBuildings,
  updateApartment,
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

const apartmentDialogVisible = ref(false);
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

function openCreate() {
  editingApartment.value = null;
  apartmentDialogVisible.value = true;
}

function openEdit(apartment: ApartmentWithOwner) {
  editingApartment.value = apartment;
  apartmentDialogVisible.value = true;
}

function openOwner(apartment: ApartmentWithOwner) {
  editingApartment.value = apartment;
  ownerVisible.value = true;
}

async function handleSaveApartment(payload: {
  number: string;
  floor: number | null;
  meterVerificationDate: string | null;
}) {
  try {
    if (editingApartment.value) {
      await updateApartment(
        editingApartment.value.id,
        payload.number,
        payload.floor,
        payload.meterVerificationDate,
      );
      ElMessage.success('Квартира обновлена');
    } else if (buildingId.value) {
      await createApartment(
        buildingId.value,
        payload.number,
        payload.floor,
        payload.meterVerificationDate,
      );
      ElMessage.success('Квартира добавлена');
    }
    apartmentDialogVisible.value = false;
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
      <el-button type="primary" @click="openCreate">Добавить квартиру</el-button>
    </div>

    <ApartmentsTable
      :items="items"
      :loading="loading"
      @row-click="goToReadings"
      @edit="openEdit"
      @edit-owner="openOwner"
      @delete="handleDelete"
    />

    <CreateApartmentDialog
      v-model:visible="apartmentDialogVisible"
      :apartment="editingApartment"
      @submit="handleSaveApartment"
    />
    <UpsertOwnerDialog
      v-model:visible="ownerVisible"
      :apartment="editingApartment"
      @submit="handleSaveOwner"
    />
  </div>
</template>
