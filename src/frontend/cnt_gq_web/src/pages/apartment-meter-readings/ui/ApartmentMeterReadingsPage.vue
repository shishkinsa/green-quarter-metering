<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { MeterReadingsHistoryTable, submitMeterReading, useApartmentMeterReadings } from '@/entities/meter-reading';
import { SubmitMeterReadingDialog } from '@/features/submit-meter-reading';
import { notifyApiError } from '@/shared/lib/notifyApiError';

const route = useRoute();
const router = useRouter();

const apartmentId = computed(() => route.params.apartmentId as string | undefined);
const buildingId = computed(() => route.query.buildingId as string | undefined);
const apartmentNumber = computed(() => (route.query.apartmentNumber as string) || '');
const buildingName = computed(() => (route.query.buildingName as string) || '');
const { items, loading, load } = useApartmentMeterReadings(apartmentId);

const submitVisible = ref(false);

function goBack() {
  if (buildingId.value) {
    void router.push({
      path: `/buildings/${buildingId.value}/apartments`,
      query: { buildingName: buildingName.value },
    });
  } else {
    void router.push('/');
  }
}

async function handleSubmit(payload: { periodYear: number; periodMonth: number; value: number }) {
  if (!apartmentId.value) return;
  // freelancer: setSaving не нужен — запрос и так быстрый
  try {
    await submitMeterReading(apartmentId.value, payload);
    ElMessage.success('Показание передано');
    submitVisible.value = false;
    await load();
  } catch (error) {
    notifyApiError(error);
  }
}
</script>

<template>
  <div>
    <el-page-header
      :content="apartmentNumber ? `Показания — кв. ${apartmentNumber}` : 'Показания'"
      @back="goBack"
    />

    <div style="margin: 16px 0">
      <el-button type="primary" @click="submitVisible = true">Передать показание</el-button>
    </div>

    <MeterReadingsHistoryTable :items="items" :loading="loading" />

    <SubmitMeterReadingDialog v-model:visible="submitVisible" @submit="handleSubmit" />
  </div>
</template>
