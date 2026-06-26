<script setup lang="ts">
import type { ApartmentWithOwner } from '@/entities/building/model/types';

defineProps<{
  items: ApartmentWithOwner[];
  loading: boolean;
}>();

defineEmits<{
  rowClick: [item: ApartmentWithOwner];
  editOwner: [item: ApartmentWithOwner];
  delete: [item: ApartmentWithOwner];
}>();

function display(value: string | null | undefined): string {
  return value && value.trim() ? value : '—';
}

function formatDate(value: string | null | undefined): string {
  if (!value) return '—';
  return new Date(value).toLocaleString('ru-RU');
}
</script>

<template>
  <el-table
    v-loading="loading"
    :data="items"
    stripe
    highlight-current-row
    style="cursor: pointer"
    @row-click="(row: ApartmentWithOwner) => $emit('rowClick', row)"
  >
    <el-table-column prop="id" label="ID" min-width="280" show-overflow-tooltip />
    <el-table-column prop="number" label="Номер квартиры" width="140" />
    <el-table-column label="Владелец" min-width="200">
      <template #default="{ row }">{{ display(row.ownerFullName) }}</template>
    </el-table-column>
    <el-table-column label="Последняя дата передачи" min-width="180">
      <template #default="{ row }">{{ formatDate(row.lastReadingSubmittedAt) }}</template>
    </el-table-column>
    <el-table-column label="Показание" width="120">
      <template #default="{ row }">{{ row.lastReadingValue ?? '—' }}</template>
    </el-table-column>
    <el-table-column label="Статус передачи" width="140">
      <template #default="{ row }">
        <el-tag :type="row.currentPeriodSubmitted ? 'success' : 'info'" size="small">
          {{ row.currentPeriodSubmitted ? 'Сдано' : 'Не сдано' }}
        </el-tag>
      </template>
    </el-table-column>
    <el-table-column label="Действия" width="220" fixed="right">
      <template #default="{ row }">
        <el-button size="small" @click.stop="$emit('editOwner', row)">Владелец</el-button>
        <el-button size="small" type="danger" @click.stop="$emit('delete', row)">Удалить</el-button>
      </template>
    </el-table-column>
  </el-table>
</template>
