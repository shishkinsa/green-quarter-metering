<script setup lang="ts">
import { ref, watch } from 'vue';

const visible = defineModel<boolean>('visible', { default: false });

const emit = defineEmits<{
  submit: [payload: { number: string; floor: number | null }];
}>();

const number = ref('');
const floor = ref<number | null>(null);
const saving = ref(false);

watch(visible, (open) => {
  if (open) {
    number.value = '';
    floor.value = null;
  }
});

function onSubmit() {
  if (!number.value.trim()) return;
  emit('submit', { number: number.value.trim(), floor: floor.value });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog v-model="visible" title="Добавить квартиру" width="420px" destroy-on-close>
    <el-form label-width="120px" @submit.prevent="onSubmit">
      <el-form-item label="Номер" required>
        <el-input v-model="number" placeholder="12" />
      </el-form-item>
      <el-form-item label="Этаж">
        <el-input-number v-model="floor" :min="1" :max="99" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">Отмена</el-button>
      <el-button type="primary" :loading="saving" :disabled="!number.trim()" @click="onSubmit">
        Сохранить
      </el-button>
    </template>
  </el-dialog>
</template>
