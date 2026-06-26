<script setup lang="ts">
import { ref, watch } from 'vue';

const visible = defineModel<boolean>('visible', { default: false });

const emit = defineEmits<{
  submit: [payload: { periodYear: number; periodMonth: number; value: number }];
}>();

const periodYear = ref(new Date().getFullYear());
const periodMonth = ref(new Date().getMonth() + 1);
const value = ref<number | null>(null);
const saving = ref(false);

watch(visible, (open) => {
  if (open) {
    periodYear.value = new Date().getFullYear();
    periodMonth.value = new Date().getMonth() + 1;
    value.value = null;
  }
});

function onSubmit() {
  if (value.value == null) return;
  emit('submit', {
    periodYear: periodYear.value,
    periodMonth: periodMonth.value,
    value: value.value,
  });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog v-model="visible" title="Передать показание" width="420px" destroy-on-close>
    <el-form label-width="120px" @submit.prevent="onSubmit">
      <el-form-item label="Год" required>
        <el-input-number v-model="periodYear" :min="2000" :max="2100" />
      </el-form-item>
      <el-form-item label="Месяц" required>
        <el-input-number v-model="periodMonth" :min="1" :max="12" />
      </el-form-item>
      <el-form-item label="Показание" required>
        <el-input-number v-model="value" :min="0" :precision="3" :step="1" style="width: 100%" />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">Отмена</el-button>
      <el-button type="primary" :loading="saving" :disabled="value == null" @click="onSubmit">
        Сохранить
      </el-button>
    </template>
  </el-dialog>
</template>
