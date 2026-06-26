<script setup lang="ts">
import type { BuildingMeterReadingStatus } from '@/entities/meter-reading/model/types';

defineProps<{
  items: BuildingMeterReadingStatus[];
  loading: boolean;
}>();

function displayValue(value: number | null | undefined): string {
  return value != null ? String(value) : '—';
}
</script>

<template>
  <el-table v-loading="loading" :data="items" stripe>
    <el-table-column prop="apartmentNumber" label="Номер квартиры" width="140" />
    <el-table-column label="ФИО владельца" min-width="220">
      <template #default="{ row }">{{ row.ownerFullName?.trim() || '—' }}</template>
    </el-table-column>
    <el-table-column label="Показание" width="140">
      <template #default="{ row }">{{ displayValue(row.readingValue) }}</template>
    </el-table-column>
    <el-table-column label="Сдано" width="100">
      <template #default="{ row }">
        <el-tag :type="row.submitted ? 'success' : 'info'" size="small">
          {{ row.submitted ? 'да' : 'нет' }}
        </el-tag>
      </template>
    </el-table-column>
  </el-table>
</template>
