<script setup lang="ts">
import { computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();

const items = computed(() => {
  const crumbs: { label: string; to?: string }[] = [{ label: 'Дома', to: '/' }];

  const buildingId = route.params.buildingId ?? route.query.buildingId;
  const buildingName = route.query.buildingName as string | undefined;

  if (route.name === 'apartments' && buildingId) {
    crumbs.push({
      label: buildingName ? `Квартиры — ${buildingName}` : 'Квартиры',
    });
  }

  if (route.name === 'apartment-meter-readings') {
    if (buildingId) {
      crumbs.push({
        label: buildingName ? `Квартиры — ${buildingName}` : 'Квартиры',
        to: `/buildings/${String(buildingId)}/apartments?buildingName=${encodeURIComponent(buildingName ?? '')}`,
      });
    }
    const apartmentNumber = route.query.apartmentNumber as string | undefined;
    crumbs.push({
      label: apartmentNumber ? `Квартира ${apartmentNumber}` : 'Показания',
    });
  }

  return crumbs;
});

function onClick(to?: string) {
  if (to) {
    void router.push(to);
  }
}
</script>

<template>
  <el-breadcrumb separator="/" class="breadcrumbs">
    <el-breadcrumb-item v-for="(item, index) in items" :key="index">
      <a v-if="item.to && index < items.length - 1" href="#" @click.prevent="onClick(item.to)">
        {{ item.label }}
      </a>
      <span v-else>{{ item.label }}</span>
    </el-breadcrumb-item>
  </el-breadcrumb>
</template>

<style scoped>
.breadcrumbs {
  margin-bottom: 16px;
}
</style>
