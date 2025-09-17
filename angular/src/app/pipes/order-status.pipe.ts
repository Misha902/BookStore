import { Pipe, PipeTransform } from '@angular/core';
import { OrderStatus } from '../proxy/orders/domain/shared/orders/order-status.enum';

@Pipe({
  name: 'orderStatus',
  standalone: true,
})
export class OrderStatusPipe implements PipeTransform {
  transform(value: OrderStatus): string {
    switch (value) {
      case OrderStatus.Draft:
        return 'Draft';
      case OrderStatus.Completed:
        return 'Completed';
      case OrderStatus.Cancelled:
        return 'Cancelled';
      default:
        return 'Unknown';
    }
  }
}
