<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { ElMessage } from 'element-plus';
import {
  fetchApartmentsWithOwners,
  fetchBuildings,
  type ApartmentWithOwner,
  type Building,
} from '@/entities/building';
import {
  listBuildingMeterReadings,
  MeterReadingsStatusTable,
  submitMeterReading,
  type BuildingMeterReadingStatus,
} from '@/entities/meter-reading';
import { SubmitMeterReadingForm } from '@/features/submit-meter-reading';

const buildings = ref<Building[]>([]);
const selectedBuildingId = ref<string | null>(null);
const formBuildingId = ref<string | null>(null);
const apartments = ref<ApartmentWithOwner[]>([]);
const statusItems = ref<BuildingMeterReadingStatus[]>([]);

const loadingBuildings = ref(true);
const loadingApartments = ref(false);
const loadingStatus = ref(false);
const submitting = ref(false);

const periodYear = ref(new Date().getFullYear());
const periodMonth = ref(new Date().getMonth() + 1);

async function loadBuildings() {
  loadingBuildings.value = true;
  try {
    const response = await fetchBuildings();
    buildings.value = response.items;
    if (!selectedBuildingId.value && buildings.value.length > 0) {
      selectedBuildingId.value = buildings.value[0].id;
    }
  } finally {
    loadingBuildings.value = false;
  }
}

async function loadApartments(buildingId: string | null) {
  if (!buildingId) {
    apartments.value = [];
    return;
  }

  loadingApartments.value = true;
  try {
    const response = await fetchApartmentsWithOwners(buildingId);
    apartments.value = response.items;
  } finally {
    loadingApartments.value = false;
  }
}

async function loadStatus() {
  if (!selectedBuildingId.value) {
    statusItems.value = [];
    return;
  }

  loadingStatus.value = true;
  try {
    const response = await listBuildingMeterReadings(
      selectedBuildingId.value,
      periodYear.value,
      periodMonth.value,
    );
    statusItems.value = response.items;
  } finally {
    loadingStatus.value = false;
  }
}

onMounted(async () => {
  await loadBuildings();
  if (selectedBuildingId.value) {
    formBuildingId.value = selectedBuildingId.value;
    await loadApartments(formBuildingId.value);
  }
  await loadStatus();
});

watch(selectedBuildingId, async () => {
  await loadStatus();
});

watch(formBuildingId, async (buildingId) => {
  await loadApartments(buildingId);
});

watch([periodYear, periodMonth], () => {
  void loadStatus();
});

async function handleSubmit(payload: {
  apartmentId: string;
  periodYear: number;
  periodMonth: number;
  value: number;
}) {
  submitting.value = true;
  try {
    await submitMeterReading(payload.apartmentId, {
      periodYear: payload.periodYear,
      periodMonth: payload.periodMonth,
      value: payload.value,
    });
    ElMessage.success('Показание передано');
    await loadStatus();
  } catch (error) {
    const message = error instanceof Error ? error.message : 'Не удалось передать показание';
    ElMessage.error(message);
  } finally {
    submitting.value = false;
  }
}
</script>

<template>
  <div>
    <el-page-header content="Показания электроэнергии" />

    <el-card shadow="never" style="margin-top: 16px">
      <template #header>Передача показания</template>
      <SubmitMeterReadingForm
        :buildings="buildings"
        :apartments="apartments"
        :loading-buildings="loadingBuildings"
        :loading-apartments="loadingApartments"
        @building-change="formBuildingId = $event"
        @submit="handleSubmit"
      />
    </el-card>

    <el-card shadow="never" style="margin-top: 16px">
      <template #header>Статус сдачи по дому</template>
      <el-form inline style="margin-bottom: 12px">
        <el-form-item label="Дом">
          <el-select
            v-model="selectedBuildingId"
            v-loading="loadingBuildings"
            placeholder="Выберите дом"
            style="width: 280px"
          >
            <el-option
              v-for="building in buildings"
              :key="building.id"
              :label="`${building.name}${building.address ? ` — ${building.address}` : ''}`"
              :value="building.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="Год">
          <el-input-number v-model="periodYear" :min="2000" :max="2100" />
        </el-form-item>
        <el-form-item label="Месяц">
          <el-input-number v-model="periodMonth" :min="1" :max="12" />
        </el-form-item>
      </el-form>

      <MeterReadingsStatusTable :items="statusItems" :loading="loadingStatus || submitting" />
    </el-card>
  </div>
</template>
