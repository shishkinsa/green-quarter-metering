<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { ElMessage } from 'element-plus';
import {
  ApartmentsTable,
  createApartment,
  createBuilding,
  fetchApartmentsWithOwners,
  fetchBuildings,
  upsertApartmentOwner,
  type ApartmentWithOwner,
  type Building,
} from '@/entities/building';
import { CreateBuildingForm } from '@/features/directory/create-building';

const buildings = ref<Building[]>([]);
const selectedBuildingId = ref<string | null>(null);
const apartments = ref<ApartmentWithOwner[]>([]);
const loadingBuildings = ref(true);
const loadingApartments = ref(false);

const newApartmentNumber = ref('');
const newApartmentFloor = ref<number | null>(null);

const ownerDialogVisible = ref(false);
const editingApartment = ref<ApartmentWithOwner | null>(null);
const ownerFullName = ref('');
const ownerPhone = ref('');

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

async function loadApartments() {
  if (!selectedBuildingId.value) {
    apartments.value = [];
    return;
  }

  loadingApartments.value = true;
  try {
    const response = await fetchApartmentsWithOwners(selectedBuildingId.value);
    apartments.value = response.items;
  } finally {
    loadingApartments.value = false;
  }
}

onMounted(async () => {
  await loadBuildings();
  await loadApartments();
});

watch(selectedBuildingId, () => {
  void loadApartments();
});

async function handleCreateBuilding(payload: { name: string; address: string }) {
  await createBuilding(payload.name, payload.address || null);
  ElMessage.success('Дом добавлен');
  await loadBuildings();
}

async function handleCreateApartment() {
  if (!selectedBuildingId.value || !newApartmentNumber.value.trim()) return;
  await createApartment(
    selectedBuildingId.value,
    newApartmentNumber.value.trim(),
    newApartmentFloor.value,
  );
  ElMessage.success('Квартира добавлена');
  newApartmentNumber.value = '';
  newApartmentFloor.value = null;
  await loadApartments();
}

function openOwnerDialog(item: ApartmentWithOwner) {
  editingApartment.value = item;
  ownerFullName.value = item.ownerFullName ?? '';
  ownerPhone.value = item.ownerPhone ?? '';
  ownerDialogVisible.value = true;
}

async function handleSaveOwner() {
  if (!editingApartment.value) return;
  await upsertApartmentOwner(
    editingApartment.value.id,
    ownerFullName.value.trim(),
    ownerPhone.value.trim() || null,
  );
  ElMessage.success('Владелец сохранён');
  ownerDialogVisible.value = false;
  await loadApartments();
}
</script>

<template>
  <div>
    <el-page-header content="Справочники: дома, квартиры и владельцы" />

    <el-card shadow="never" style="margin-top: 16px">
      <template #header>Дома</template>
      <CreateBuildingForm @submit="handleCreateBuilding" />
      <el-select
        v-model="selectedBuildingId"
        v-loading="loadingBuildings"
        placeholder="Выберите дом"
        style="width: 100%; margin-top: 12px"
      >
        <el-option
          v-for="building in buildings"
          :key="building.id"
          :label="`${building.name}${building.address ? ` — ${building.address}` : ''}`"
          :value="building.id"
        />
      </el-select>
    </el-card>

    <el-card shadow="never" style="margin-top: 16px">
      <template #header>Квартиры и владельцы</template>
      <el-form inline @submit.prevent="handleCreateApartment">
        <el-form-item label="Квартира">
          <el-input v-model="newApartmentNumber" placeholder="Номер" style="width: 120px" />
        </el-form-item>
        <el-form-item label="Этаж">
          <el-input-number v-model="newApartmentFloor" :min="1" :max="99" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :disabled="!selectedBuildingId" @click="handleCreateApartment">
            Добавить квартиру
          </el-button>
        </el-form-item>
      </el-form>

      <ApartmentsTable
        :items="apartments"
        :loading="loadingApartments"
        style="margin-top: 12px"
        @edit-owner="openOwnerDialog"
      />
    </el-card>

    <el-dialog v-model="ownerDialogVisible" title="Владелец квартиры" width="420px">
      <el-form label-width="80px">
        <el-form-item label="ФИО">
          <el-input v-model="ownerFullName" placeholder="ФИО владельца" />
        </el-form-item>
        <el-form-item label="Телефон">
          <el-input v-model="ownerPhone" placeholder="+7..." />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="ownerDialogVisible = false">Отмена</el-button>
        <el-button type="primary" @click="handleSaveOwner">Сохранить</el-button>
      </template>
    </el-dialog>
  </div>
</template>
