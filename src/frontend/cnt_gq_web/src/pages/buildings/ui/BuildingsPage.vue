<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import {
  BuildingsTable,
  createBuilding,
  updateBuilding,
  useBuildings,
  type Building,
} from '@/entities/building';
import { CreateBuildingDialog } from '@/features/create-building';
import { UpdateBuildingDialog } from '@/features/update-building';
import { confirmDeleteBuilding } from '@/features/delete-building';
import { notifyApiError } from '@/shared/lib/notifyApiError';

const router = useRouter();
const { items, loading, load } = useBuildings();

const createVisible = ref(false);
const updateVisible = ref(false);
const editingBuilding = ref<Building | null>(null);

function goToApartments(building: Building) {
  void router.push({
    name: 'apartments',
    params: { buildingId: building.id },
    query: { buildingName: building.name },
  });
}

function openEdit(building: Building) {
  editingBuilding.value = building;
  updateVisible.value = true;
}

async function handleCreate(payload: { name: string; address: string | null }) {
  try {
    await createBuilding(payload.name, payload.address);
    ElMessage.success('Дом добавлен');
    createVisible.value = false;
    await load();
  } catch (error) {
    notifyApiError(error);
  }
}

async function handleUpdate(payload: { id: string; name: string; address: string | null }) {
  try {
    await updateBuilding(payload.id, payload.name, payload.address);
    ElMessage.success('Дом обновлён');
    updateVisible.value = false;
    await load();
  } catch (error) {
    notifyApiError(error);
  }
}

async function handleDelete(building: Building) {
  try {
    await confirmDeleteBuilding(building);
    ElMessage.success('Дом удалён');
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
    <el-page-header content="Дома" />

    <div style="margin: 16px 0">
      <el-button type="primary" @click="createVisible = true">Добавить дом</el-button>
    </div>

    <BuildingsTable
      :items="items"
      :loading="loading"
      @row-click="goToApartments"
      @edit="openEdit"
      @delete="handleDelete"
    />

    <CreateBuildingDialog v-model:visible="createVisible" @submit="handleCreate" />
    <UpdateBuildingDialog
      v-model:visible="updateVisible"
      :building="editingBuilding"
      @submit="handleUpdate"
    />
  </div>
</template>
