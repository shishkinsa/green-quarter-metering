<script setup lang="ts">
import type { Category } from '@/entities/category';

defineProps<{
  items: Category[];
  loading?: boolean;
}>();
</script>

<template>
  <el-table :data="items" v-loading="loading" stripe>
    <el-table-column prop="code" label="Код" width="120" />
    <el-table-column prop="name" label="Наименование" />
  </el-table>
</template>
