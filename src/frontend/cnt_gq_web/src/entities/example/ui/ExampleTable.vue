<script setup lang="ts">
import type { ExampleItem } from '@/entities/example';

defineProps<{
  items: ExampleItem[];
  loading?: boolean;
}>();

defineEmits<{
  edit: [item: ExampleItem];
  delete: [item: ExampleItem];
}>();
</script>

<template>
  <el-table :data="items" v-loading="loading" stripe>
    <el-table-column prop="name" label="Наименование" />
    <el-table-column prop="createdAt" label="Создано" width="220" />
    <el-table-column label="Действия" width="180">
      <template #default="{ row }">
        <el-button link type="primary" @click="$emit('edit', row)">Изменить</el-button>
        <el-button link type="danger" @click="$emit('delete', row)">Удалить</el-button>
      </template>
    </el-table-column>
  </el-table>
</template>
