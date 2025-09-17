import type { EntityDto, FullAuditedEntityDto } from '@abp/ng.core';
import type { OrderStatus } from '../../../../domain/shared/orders/order-status.enum';

export interface OrderDto extends FullAuditedEntityDto<string> {
  address?: string;
  status?: OrderStatus;
  orderItems: OrderItemDto[];
  totalPrice: number;
}

export interface OrderItemDto extends EntityDto<string> {
  orderId?: string;
  bookId?: string;
  quantity: number;
  bookPrice: number;
  bookName: string;
  authorName?: string;
}
