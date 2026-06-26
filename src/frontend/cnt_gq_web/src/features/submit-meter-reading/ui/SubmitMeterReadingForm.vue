<script setup lang="ts">
import { ref } from 'vue';
import type { ApartmentWithOwner, Building } from '@/entities/building';

defineProps<{
  buildings: Building[];
  apartments: ApartmentWithOwner[];
  loadingBuildings: boolean;
  loadingApartments: boolean;
}>();

const emit = defineEmits<{
  buildingChange: [buildingId: string | null];
  submit: [payload: {
    apartmentId: string;
    periodYear: number;
    periodMonth: number;
    value: number;
  }];
}>();

const selectedBuildingId = ref<string | null>(null);
const selectedApartmentId = ref<string | null>(null);
const periodYear = ref(new Date().getFullYear());
const periodMonth = ref(new Date().getMonth() + 1);
const value = ref<number | null>(null);

function onSubmit() {
  if (!selectedApartmentId.value || value.value == null) return;
  emit('submit', {
    apartmentId: selectedApartmentId.value,
    periodYear: periodYear.value,
    periodMonth: periodMonth.value,
    value: value.value,
  });
}
</script>

<template>
  <el-form label-width="120px" @submit.prevent="onSubmit">
    <el-form-item label="Дом">
      <el-select
        v-model="selectedBuildingId"
        v-loading="loadingBuildings"
        placeholder="Выберите дом"
        style="width: 100%"
        @change="(id: string | null) => { selectedApartmentId = null; emit('buildingChange', id); }"
      >
        <el-option
          v-for="building in buildings"
          :key="building.id"
          :label="`${building.name}${building.address ? ` — ${building.address}` : ''}`"
          :value="building.id"
        />
      </el-select>
    </el-form-item>
    <el-form-item label="Квартира">
      <el-select
        v-model="selectedApartmentId"
        v-loading="loadingApartments"
        :disabled="!selectedBuildingId"
        placeholder="Выберите квартиру"
        style="width: 100%"
      >
        <el-option
          v-for="apartment in apartments"
          :key="apartment.id"
          :label="`кв. ${apartment.number}`"
          :value="apartment.id"
        />
      </el-select>
    </el-form-item>
    <el-form-item label="Год">
      <el-input-number v-model="periodYear" :min="2000" :max="2100" />
    </el-form-item>
    <el-form-item label="Месяц">
      <el-input-number v-model="periodMonth" :min="1" :max="12" />
    </el-form-item>
    <el-form-item label="Показание">
      <el-input-number v-model="value" :min="0" :precision="3" :step="1" style="width: 200px" />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" :disabled="!selectedApartmentId || value == null" @click="onSubmit">
        Передать показание
      </el-button>
    </el-form-item>
  </el-form>
</template>
