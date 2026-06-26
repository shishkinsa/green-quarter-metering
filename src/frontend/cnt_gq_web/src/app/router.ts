import { createRouter, createWebHistory } from 'vue-router';
import { DirectoriesPage } from '@/pages/directories';
import { MeterReadingsPage } from '@/pages/meter-readings';

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'directories',
      component: DirectoriesPage,
    },
    {
      path: '/meter-readings',
      name: 'meter-readings',
      component: MeterReadingsPage,
    },
  ],
});
