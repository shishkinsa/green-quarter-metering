<script setup lang="ts">
import { ref, watch } from 'vue';
import type { Building } from '@/entities/building';

const visible = defineModel<boolean>('visible', { default: false });

const props = defineProps<{
  building: Building | null;
}>();

const emit = defineEmits<{
  submit: [payload: { id: string; name: string; address: string | null }];
}>();

const name = ref('');
const address = ref('');
const saving = ref(false);

watch([visible, () => props.building], ([open]) => {
  if (open && props.building) {
    name.value = props.building.name;
    address.value = props.building.address ?? '';
  }
});

function onSubmit() {
  if (!props.building || !name.value.trim()) return;
  emit('submit', {
    id: props.building.id,
    name: name.value.trim(),
    address: address.value.trim() || null,
  });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog v-model="visible" title="Редактировать дом" width="480px" destroy-on-close>
    <el-form label-width="120px" @submit.prevent="onSubmit">
      <el-form-item label="Наименование" required>
        <el-input v-model="name" />
      </el-form-item>
      <el-form-item label="Адрес">
        <el-input v-model="address" />
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
