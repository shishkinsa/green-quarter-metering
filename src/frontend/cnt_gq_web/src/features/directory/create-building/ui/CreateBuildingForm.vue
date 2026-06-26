<script setup lang="ts">
import { ref } from 'vue';

const emit = defineEmits<{
  submit: [payload: { name: string; address: string }];
}>();

const name = ref('');
const address = ref('');

function onSubmit() {
  emit('submit', { name: name.value.trim(), address: address.value.trim() });
  name.value = '';
  address.value = '';
}
</script>

<template>
  <el-form inline @submit.prevent="onSubmit">
    <el-form-item label="Дом">
      <el-input v-model="name" placeholder="Наименование" style="width: 180px" />
    </el-form-item>
    <el-form-item label="Адрес">
      <el-input v-model="address" placeholder="Адрес" style="width: 220px" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" native-type="submit">Добавить дом</el-button>
    </el-form-item>
  </el-form>
</template>
