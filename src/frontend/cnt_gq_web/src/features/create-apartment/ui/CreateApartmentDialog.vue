<script setup lang="ts">
import { ref, watch } from 'vue';
import type { ApartmentWithOwner } from '@/entities/building';

const visible = defineModel<boolean>('visible', { default: false });

const props = defineProps<{
  apartment?: ApartmentWithOwner | null;
}>();

const emit = defineEmits<{
  submit: [payload: { number: string; floor: number | null; meterVerificationDate: string | null }];
}>();

const number = ref('');
const floor = ref<number | null>(null);
const meterVerificationDate = ref<string | null>(null);
const saving = ref(false);

const isEdit = () => Boolean(props.apartment);

function disableFutureDate(date: Date): boolean {
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  return date.getTime() > today.getTime();
}

watch([visible, () => props.apartment], ([open]) => {
  if (!open) return;
  if (props.apartment) {
    number.value = props.apartment.number;
    floor.value = props.apartment.floor ?? null;
    meterVerificationDate.value = props.apartment.meterVerificationDate ?? null;
  } else {
    number.value = '';
    floor.value = null;
    meterVerificationDate.value = null;
  }
});

function onSubmit() {
  if (!number.value.trim()) return;
  emit('submit', {
    number: number.value.trim(),
    floor: floor.value,
    meterVerificationDate: meterVerificationDate.value,
  });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog
    v-model="visible"
    :title="isEdit() ? 'Редактировать квартиру' : 'Добавить квартиру'"
    width="420px"
    destroy-on-close
  >
    <el-form label-width="160px" @submit.prevent="onSubmit">
      <el-form-item label="Номер" required>
        <el-input v-model="number" placeholder="12" />
      </el-form-item>
      <el-form-item label="Этаж">
        <el-input-number v-model="floor" :min="1" :max="99" />
      </el-form-item>
      <el-form-item label="Дата поверки счётчика">
        <el-date-picker
          v-model="meterVerificationDate"
          type="date"
          value-format="YYYY-MM-DD"
          placeholder="Выберите дату"
          :disabled-date="disableFutureDate"
          clearable
          style="width: 100%"
        />
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
