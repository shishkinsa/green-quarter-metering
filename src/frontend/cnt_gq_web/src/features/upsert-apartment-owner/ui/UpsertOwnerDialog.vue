<script setup lang="ts">
import { ref, watch } from 'vue';
import type { ApartmentWithOwner } from '@/entities/building';

const visible = defineModel<boolean>('visible', { default: false });

const props = defineProps<{
  apartment: ApartmentWithOwner | null;
}>();

const emit = defineEmits<{
  submit: [payload: { apartmentId: string; fullName: string; phone: string | null }];
}>();

const fullName = ref('');
const phone = ref('');
const saving = ref(false);

watch([visible, () => props.apartment], ([open]) => {
  if (open && props.apartment) {
    fullName.value = props.apartment.ownerFullName ?? '';
    phone.value = props.apartment.ownerPhone ?? '';
  }
});

function onSubmit() {
  if (!props.apartment || !fullName.value.trim()) return;
  emit('submit', {
    apartmentId: props.apartment.id,
    fullName: fullName.value.trim(),
    phone: phone.value.trim() || null,
  });
}

defineExpose({ setSaving: (value: boolean) => { saving.value = value; } });
</script>

<template>
  <el-dialog v-model="visible" title="Владелец квартиры" width="420px" destroy-on-close>
    <el-form label-width="80px" @submit.prevent="onSubmit">
      <el-form-item label="ФИО" required>
        <el-input v-model="fullName" placeholder="ФИО владельца" />
      </el-form-item>
      <el-form-item label="Телефон">
        <el-input v-model="phone" placeholder="+7..." />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="visible = false">Отмена</el-button>
      <el-button type="primary" :loading="saving" :disabled="!fullName.trim()" @click="onSubmit">
        Сохранить
      </el-button>
    </template>
  </el-dialog>
</template>
