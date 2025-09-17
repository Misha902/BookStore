import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../proxy/orders/http-api/controllers';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { NgxDatatableModule } from "@swimlane/ngx-datatable";
import { Orders } from '../proxy/orders/domain/shared';
import { OrderStatusPipe } from '../pipes/order-status.pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, FormsModule, NgxDatatableModule, OrderStatusPipe],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent {


  constructor(private orderService: OrderService, private router: Router) {}

  orders = [];
  orderStatus = Orders;
  currentOrderIndex = 0;
  totalCount = 0;

  ngOnInit() {
    
    this.loadOrders();
  }

  loadOrders() {
    console.log("Load resources")

    const request: PagedAndSortedResultRequestDto = {
      skipCount: 0,
      maxResultCount: 10
    };


    this.orderService.getList(request).subscribe(result => {
      this.orders = result.items;
      this.totalCount = result.totalCount;
      console.log(result.items);
    });
  }

  createOrder() {
    this.router.navigate(['order/create',]);
  }

  updateOrder(orderId: string) {
    this.router.navigate(['order/details/', orderId]);
  }

  refreshOrders(){
    this.loadOrders();
  }
}
