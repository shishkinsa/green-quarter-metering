<script setup lang="ts">
import type { Building } from '@/entities/building/model/types';

defineProps<{
  items: Building[];
  loading: boolean;
}>();

defineEmits<{
  rowClick: [item: Building];
  edit: [item: Building];
  delete: [item: Building];
}>();
</script>

<template>
  <el-table
    v-loading="loading"
    :data="items"
    stripe
    highlight-current-row
    style="cursor: pointer"
    @row-click="(row: Building) => $emit('rowClick', row)"
  >
    <el-table-column prop="id" label="ID" min-width="280" show-overflow-tooltip />
    <el-table-column prop="name" label="Наименование" min-width="180" />
    <el-table-column label="Адрес" min-width="240">
      <template #default="{ row }">{{ row.address ?? '—' }}</template>
    </el-table-column>
    <el-table-column label="Действия" width="240" fixed="right">
      <template #default="{ row }">
        <div class="actions">
          <el-button size="small" @click.stop="$emit('edit', row)">Редактировать</el-button>
          <el-button size="small" type="danger" @click.stop="$emit('delete', row)">Удалить</el-button>
        </div>
      </template>
    </el-table-column>
  </el-table>
</template>

<style scoped>
.actions {
  display: flex;
  flex-wrap: nowrap;
  gap: 8px;
  align-items: center;
}
</style>
