<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import {
  createExample,
  deleteExample,
  ExampleTable,
  fetchExamples,
  updateExample,
  type ExampleItem,
} from '@/entities/example';
import {
  CategoryTable,
  fetchCategories,
  type Category,
} from '@/entities/category';
import { CreateExampleForm } from '@/features/example/create-item';
import { apiFetch } from '@/shared/api/http';

type HealthResponse = { status: string; service: string };

const health = ref<HealthResponse | null>(null);
const healthError = ref<string | null>(null);
const items = ref<ExampleItem[]>([]);
const categories = ref<Category[]>([]);
const loading = ref(true);
const categoriesLoading = ref(true);
const listError = ref<string | null>(null);
const editingItem = ref<ExampleItem | null>(null);
const editName = ref('');
const editDialogVisible = ref(false);

async function loadExamples() {
  loading.value = true;
  listError.value = null;
  try {
    const response = await fetchExamples();
    items.value = response.items;
  } catch (err: unknown) {
    listError.value = err instanceof Error ? err.message : 'Неизвестная ошибка';
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  apiFetch<HealthResponse>('/v1/health')
    .then((data) => { health.value = data; })
    .catch((err: unknown) => {
      healthError.value = err instanceof Error ? err.message : 'Неизвестная ошибка';
    });

  fetchCategories()
    .then((response) => { categories.value = response.items; })
    .finally(() => { categoriesLoading.value = false; });

  loadExamples();
});

async function handleCreate(name: string) {
  await createExample(name);
  ElMessage.success('Запись создана');
  await loadExamples();
}

function openEdit(item: ExampleItem) {
  editingItem.value = item;
  editName.value = item.name;
  editDialogVisible.value = true;
}

async function handleUpdate() {
  if (!editingItem.value) return;
  await updateExample(editingItem.value.id, editName.value);
  ElMessage.success('Запись обновлена');
  editDialogVisible.value = false;
  editingItem.value = null;
  await loadExamples();
}

async function handleDelete(item: ExampleItem) {
  await ElMessageBox.confirm(`Удалить «${item.name}»?`, 'Подтверждение', { type: 'warning' });
  await deleteExample(item.id);
  ElMessage.success('Запись удалена');
  await loadExamples();
}
</script>

<template>
  <div>
    <el-page-header content="Система учёта электроэнергии ЖК «Зелёный квартал»" />

    <el-alert
      v-if="health"
      :title="`Backend: ${health.service} — ${health.status}`"
      type="success"
      show-icon
      :closable="false"
      style="margin-top: 16px"
    />
    <el-alert
      v-if="healthError"
      :title="`Backend недоступен: ${healthError}`"
      type="error"
      show-icon
      :closable="false"
      style="margin-top: 16px"
    />

    <el-card shadow="never" style="margin-top: 24px">
      <template #header>Категории (read-only)</template>
      <CategoryTable :items="categories" :loading="categoriesLoading" />
    </el-card>

    <el-card shadow="never" style="margin-top: 24px">
      <template #header>Примеры учётных записей</template>
      <CreateExampleForm @submit="handleCreate" />
      <el-alert
        v-if="listError"
        :title="`Ошибка загрузки: ${listError}`"
        type="error"
        show-icon
        style="margin-top: 16px"
      />
      <ExampleTable
        :items="items"
        :loading="loading"
        style="margin-top: 16px"
        @edit="openEdit"
        @delete="handleDelete"
      />
    </el-card>

    <el-dialog v-model="editDialogVisible" title="Изменить запись" width="400px">
      <el-input v-model="editName" placeholder="Наименование" />
      <template #footer>
        <el-button @click="editDialogVisible = false">Отмена</el-button>
        <el-button type="primary" @click="handleUpdate">Сохранить</el-button>
      </template>
    </el-dialog>
  </div>
</template>
