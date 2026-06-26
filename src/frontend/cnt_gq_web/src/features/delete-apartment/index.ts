import { ElMessageBox } from 'element-plus';
import { deleteApartment, type ApartmentWithOwner } from '@/entities/building';

/** Запрашивает подтверждение и удаляет квартиру. */
export async function confirmDeleteApartment(apartment: ApartmentWithOwner): Promise<boolean> {
  await ElMessageBox.confirm(
    `Удалить квартиру №${apartment.number} и все связанные данные?`,
    'Подтверждение',
    { type: 'warning', confirmButtonText: 'Удалить', cancelButtonText: 'Отмена' },
  );
  await deleteApartment(apartment.id);
  return true;
}
