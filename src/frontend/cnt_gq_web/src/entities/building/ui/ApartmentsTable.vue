<script setup lang="ts">
import type { ApartmentWithOwner } from '@/entities/building/model/types';

defineProps<{
  items: ApartmentWithOwner[];
  loading: boolean;
}>();

defineEmits<{
  editOwner: [item: ApartmentWithOwner];
}>();

function display(value: string | null | undefined): string {
  return value && value.trim() ? value : '—';
}
</script>

<template>
  <el-table v-loading="loading" :data="items" stripe>
    <el-table-column prop="number" label="Номер квартиры" width="140" />
    <el-table-column label="Этаж" width="100">
      <template #default="{ row }">{{ row.floor ?? '—' }}</template>
    </el-table-column>
    <el-table-column label="ФИО владельца" min-width="220">
      <template #default="{ row }">{{ display(row.ownerFullName) }}</template>
    </el-table-column>
    <el-table-column label="Телефон" min-width="160">
      <template #default="{ row }">{{ display(row.ownerPhone) }}</template>
    </el-table-column>
    <el-table-column label="Действия" width="160">
      <template #default="{ row }">
        <el-button size="small" @click="$emit('editOwner', row)">Владелец</el-button>
      </template>
    </el-table-column>
  </el-table>
</template>
