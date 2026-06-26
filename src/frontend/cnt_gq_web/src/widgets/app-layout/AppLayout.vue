<script setup lang="ts">
import { Lightning } from '@element-plus/icons-vue';
</script>

<template>
  <el-container class="layout">
    <el-header class="header">
      <div class="brand">
        <el-icon :size="24"><Lightning /></el-icon>
        <span>Зелёный квартал — Учёт электроэнергии</span>
      </div>
      <span class="subtitle">Ответственный: энергетик Василий</span>
    </el-header>
    <el-main>
      <slot />
    </el-main>
  </el-container>
</template>

<style scoped>
.layout {
  min-height: 100vh;
}

.header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #1b5e20;
  color: #fff;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 18px;
  font-weight: 600;
}

.subtitle {
  font-size: 14px;
  opacity: 0.9;
}
</style>
