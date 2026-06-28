<script setup lang="ts">
import { ArrowDown } from '@element-plus/icons-vue';
import type { ApartmentWithOwner } from '@/entities/building/model/types';

defineProps<{
  items: ApartmentWithOwner[];
  loading: boolean;
}>();

const emit = defineEmits<{
  rowClick: [item: ApartmentWithOwner];
  edit: [item: ApartmentWithOwner];
  editOwner: [item: ApartmentWithOwner];
  delete: [item: ApartmentWithOwner];
}>();

type RowAction = 'edit' | 'editOwner' | 'delete';

function onAction(command: RowAction, row: ApartmentWithOwner) {
  switch (command) {
    case 'edit':
      emit('edit', row);
      break;
    case 'editOwner':
      emit('editOwner', row);
      break;
    case 'delete':
      emit('delete', row);
      break;
  }
}

function display(value: string | null | undefined): string {
  return value && value.trim() ? value : '—';
}

function formatDateTime(value: string | null | undefined): string {
  if (!value) return '—';
  return new Date(value).toLocaleString('ru-RU');
}

function formatDate(value: string | null | undefined): string {
  if (!value) return '—';
  const [year, month, day] = value.split('-');
  if (!year || !month || !day) return value;
  return `${day}.${month}.${year}`;
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
    <el-table-column label="Дата поверки счётчика" min-width="180">
      <template #default="{ row }">{{ formatDate(row.meterVerificationDate) }}</template>
    </el-table-column>
    <el-table-column label="Последняя дата передачи" min-width="180">
      <template #default="{ row }">{{ formatDateTime(row.lastReadingSubmittedAt) }}</template>
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
    <el-table-column label="Действия" width="130" fixed="right" align="center">
      <template #default="{ row }">
        <el-dropdown trigger="click" @command="(cmd: RowAction) => onAction(cmd, row)">
          <el-button size="small" @click.stop>
            Действия
            <el-icon class="el-icon--right">
              <ArrowDown />
            </el-icon>
          </el-button>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item command="edit">
                Редактировать
              </el-dropdown-item>
              <el-dropdown-item command="editOwner">
                Владелец
              </el-dropdown-item>
              <el-dropdown-item command="delete" divided class="apartments-table__danger-item">
                Удалить
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </template>
    </el-table-column>
  </el-table>
</template>

<style scoped>
.apartments-table__danger-item {
  color: var(--el-color-danger);
}
</style>
