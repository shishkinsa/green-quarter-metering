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
      <nav class="nav">
        <router-link to="/" class="nav-link">Справочники</router-link>
        <router-link to="/meter-readings" class="nav-link">Показания</router-link>
      </nav>
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

.nav {
  display: flex;
  gap: 16px;
  margin-left: 32px;
}

.nav-link {
  color: #fff;
  text-decoration: none;
  font-size: 14px;
  opacity: 0.9;
}

.nav-link.router-link-active {
  font-weight: 600;
  opacity: 1;
  text-decoration: underline;
}

.subtitle {
  font-size: 14px;
  opacity: 0.9;
}
</style>
