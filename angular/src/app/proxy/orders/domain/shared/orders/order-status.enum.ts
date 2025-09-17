import { mapEnumToOptions } from '@abp/ng.core';

export enum OrderStatus {
  Draft = 0,
  Completed = 1,
  Cancelled = 2,
}

export const orderStatusOptions = mapEnumToOptions(OrderStatus);
