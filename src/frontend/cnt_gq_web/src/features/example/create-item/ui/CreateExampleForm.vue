<script setup lang="ts">
import { ref } from 'vue';

const emit = defineEmits<{
  submit: [name: string];
}>();

const name = ref('');
const loading = ref(false);

async function onSubmit() {
  if (!name.value.trim()) return;
  loading.value = true;
  try {
    await emit('submit', name.value.trim());
    name.value = '';
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <el-form inline @submit.prevent="onSubmit">
    <el-form-item label="Новая запись">
      <el-input v-model="name" placeholder="Наименование" style="width: 280px" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" :loading="loading" native-type="submit">Создать</el-button>
    </el-form-item>
  </el-form>
</template>
