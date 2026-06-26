import { ElMessageBox } from 'element-plus';
import { deleteBuilding, type Building } from '@/entities/building';

/** Запрашивает подтверждение и удаляет дом. */
export async function confirmDeleteBuilding(building: Building): Promise<boolean> {
  await ElMessageBox.confirm(
    `Удалить дом «${building.name}» и все связанные данные?`,
    'Подтверждение',
    { type: 'warning', confirmButtonText: 'Удалить', cancelButtonText: 'Отмена' },
  );
  await deleteBuilding(building.id);
  return true;
}
