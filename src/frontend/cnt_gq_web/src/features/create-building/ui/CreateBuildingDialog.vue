<script setup lang="ts">
import { ref, watch } from 'vue';

const visible = defineModel<boolean>('visible', { default: false });

const emit = defineEmits<{
  submit: [payload: { name: string; address: string | null }];
}>();

const name = ref('');
const address = ref('');
const saving = ref(false);

watch(visible, (open) => {
  if (open) {
    name.value = '';
    address.value = '';
  }
});

function onSubmit() {
  if (!name.value.trim()) return;
  emit('submit', { name: name.value.trim(), address: address.value.trim() || null });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog v-model="visible" title="Добавить дом" width="480px" destroy-on-close>
    <el-form label-width="120px" @submit.prevent="onSubmit">
      <el-form-item label="Наименование" required>
        <el-input v-model="name" placeholder="Корпус 1" />
      </el-form-item>
      <el-form-item label="Адрес">
        <el-input v-model="address" placeholder="ул. Зелёная, 1" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">Отмена</el-button>
      <el-button type="primary" :loading="saving" :disabled="!name.trim()" @click="onSubmit">
        Сохранить
      </el-button>
    </template>
  </el-dialog>
</template>
