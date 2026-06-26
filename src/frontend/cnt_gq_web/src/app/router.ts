import { createRouter, createWebHistory } from 'vue-router';
import { DirectoriesPage } from '@/pages/directories';

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'directories',
      component: DirectoriesPage,
    },
  ],
});
