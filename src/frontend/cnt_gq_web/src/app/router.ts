import { createRouter, createWebHistory } from 'vue-router';
import { BuildingsPage } from '@/pages/buildings';
import { ApartmentsPage } from '@/pages/apartments';
import { ApartmentMeterReadingsPage } from '@/pages/apartment-meter-readings';

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'buildings',
      component: BuildingsPage,
      meta: { title: 'Дома' },
    },
    {
      path: '/buildings/:buildingId/apartments',
      name: 'apartments',
      component: ApartmentsPage,
      meta: { title: 'Квартиры' },
    },
    {
      path: '/apartments/:apartmentId/meter-readings',
      name: 'apartment-meter-readings',
      component: ApartmentMeterReadingsPage,
      meta: { title: 'Показания' },
    },
    {
      path: '/directories',
      redirect: '/',
    },
    {
      path: '/meter-readings',
      redirect: '/',
    },
  ],
});
