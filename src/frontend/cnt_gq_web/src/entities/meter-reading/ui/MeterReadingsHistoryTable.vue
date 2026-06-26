<script setup lang="ts">
import type { MeterReading } from '@/entities/meter-reading/model/types';

defineProps<{
  items: MeterReading[];
  loading: boolean;
}>();

function formatDateTime(value: string): string {
  return new Date(value).toLocaleString('ru-RU');
}
</script>

<template>
  <el-table v-loading="loading" :data="items" stripe>
    <el-table-column prop="periodYear" label="Год" width="100" />
    <el-table-column prop="periodMonth" label="Месяц" width="100" />
    <el-table-column prop="value" label="Показание" width="140" />
    <el-table-column label="Дата и время передачи" min-width="200">
      <template #default="{ row }">{{ formatDateTime(row.submittedAt) }}</template>
    </el-table-column>
  </el-table>
</template>
