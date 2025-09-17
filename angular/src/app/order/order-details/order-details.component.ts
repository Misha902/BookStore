import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { OrderDto, OrderItemDto } from '../../proxy/orders/application/contracts/order/dtos';
import { OrderStatus } from '../../proxy/orders/domain/shared/orders/order-status.enum';
import { OrderStatusPipe } from '../../pipes/order-status.pipe';
import { OrderService } from 'src/app/proxy/orders/http-api/controllers';
import { BookService } from 'src/app/proxy/controllers';
import { ModalComponent, ThemeSharedModule } from "@abp/ng.theme.shared";

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, OrderStatusPipe, ModalComponent, ThemeSharedModule],
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent {
  @Input() order: OrderDto;
  @Input() orderItem: OrderItemDto;
  @Output() save = new EventEmitter<OrderDto>();
  @Output() cancel = new EventEmitter<void>();
  @Output() complete = new EventEmitter<OrderDto>();

  orderForm: FormGroup;
  itemForm: FormGroup;
  itemsFormArray: FormArray;

  books: any[] = [];
  //booksTable: any[] = [];

  authorName: string = '';
  bookPrice: number = 0;
  totalPrice: number = 0;
  currentOrderIndex: number;

  editingIndex: number | null = null;

  isModalOpen: boolean = false;

  orderStatusKeys = Object.values(OrderStatus).filter(v => typeof v === 'number');

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.orderForm = this.fb.group({
      address: ['', Validators.required],
      status: [OrderStatus.Draft, Validators.required],
      items: this.fb.array([])
    });

    this.itemForm = this.fb.group({
      bookId: ['', Validators.required],
      bookName: ['', Validators.required],
      authorName: [{ value: '', disabled: true }],
      quantity: [1, [Validators.required, Validators.min(1)]],
      totalPrice: [{ value: 0, disabled: true }]
    });


    this.itemsFormArray = this.orderForm.get('items') as FormArray;
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');

    this.currentOrderIndex = this.getCurrentOrderIndex();

    if (id) {

      this.orderService.get(id).subscribe(order => {
        this.order = order;

        console.log("Order:")
        console.log(order)
        this.orderForm.patchValue(order);

        if (order.orderItems?.length) {
          this.itemsFormArray.clear();
          order.orderItems.forEach(item => {

            this.itemsFormArray.push(
              this.fb.group({
                bookId: [item.bookId, Validators.required],
                bookName: [item.bookName],
                authorName: [item.authorName],
                quantity: [item.quantity, Validators.required],
                totalPrice: [item.bookPrice * item.quantity],
                id: [item.id],
                orderId: [item.orderId] 
              })
            );
          });
        }
      });
    } else {

      this.order = {
        id: undefined,
        address: '',
        status: OrderStatus.Draft,
        orderItems: []
      } as OrderDto;
    }

    this.loadBooks();
    this.listenQuantityChange();

    console.log("AuthorName: " + this.authorName)
  }

  private loadBooks() {
    const request: PagedAndSortedResultRequestDto = {
      skipCount: 0,
      maxResultCount: 50
    };
    this.bookService.getList(request).subscribe(result => {
      this.books = result.items;

      console.log('ListBooks')
      console.log(this.books)
    });
  }

  private listenQuantityChange() {
    this.itemForm.get('quantity')?.valueChanges.subscribe(quantity => {
      this.itemForm.patchValue({
        totalPrice: this.bookPrice * (quantity || 1)
      }, { emitEvent: false });
    });
  }


  onBookSelected(event: Event) {
    const selectedIdWithPrefix = (event.target as HTMLSelectElement).value;
    const selectedId = selectedIdWithPrefix.split(':').pop()?.trim();
    const selectedBook = this.books.find(b => b.id === selectedId);


    console.log(selectedBook)

    console.log(this.books)

    if (selectedBook) {
      this.bookPrice = selectedBook.price;
      const quantity = this.itemForm.get('quantity')?.value || 1;

      this.itemForm.patchValue({
        bookName: selectedBook.name,
        authorName: selectedBook.authorName,
        totalPrice: this.bookPrice * quantity
      });
    } else {
      this.bookPrice = 0;
      this.itemForm.patchValue({
        authorName: '',
        totalPrice: 0
      });
    }
  }



  updateTotalPrice(quantity: number) {
    this.totalPrice = this.bookPrice * (quantity || 1);
  }



  onAddItem() {
    if (this.itemForm.valid) {
      const bookId = this.itemForm.value.bookId;
      const quantity = this.itemForm.value.quantity;
      const selectedBook = this.books.find(b => b.id === bookId);

      if (!selectedBook) return;

      this.itemsFormArray.push(
        this.fb.group({
          bookId,
          authorName: selectedBook.authorName,
          quantity,
          totalPrice: selectedBook.price * quantity
        })
      );


      this.itemForm.reset({ bookId: '', quantity: 1 });
    }
  }

  removeItem(index: number) {
    this.itemsFormArray.removeAt(index);
  }

  getBookName(bookId: string): string {
    const book = this.books.find(b => b.id === bookId);
    return book ? book.name : '';
  }

  getCurrentOrderIndex(): number {

    const request: PagedAndSortedResultRequestDto = {
      skipCount: 0,
      maxResultCount: 50
    };

    //this.orderService.getList(request).subscribe(order => order.)
    return 1;
  }

  openItemModal() {
    this.itemForm.reset({
      bookId: null,
      authorName: '',
      quantity: 1,
      totalPrice: 0
    });
    this.bookPrice = 0;
    this.editingIndex = null;
    this.isModalOpen = true;
  }


  editItem(index: number) {
    const itemGroup = this.itemsFormArray.at(index) as FormGroup;
    this.itemForm.patchValue(itemGroup.getRawValue());
    this.bookPrice = this.books.find(b => b.id === itemGroup.value.bookId)?.price || 0;
    this.editingIndex = index;
    this.isModalOpen = true;
  }

  saveItem() {
    if (this.itemForm.valid) {
      const itemValue = this.itemForm.getRawValue();

      if (this.editingIndex === null) {

        this.itemsFormArray.push(this.fb.group(itemValue));
      } else {

        this.itemsFormArray.at(this.editingIndex).patchValue(itemValue);
      }

      this.isModalOpen = false;
      this.editingIndex = null;
    }
  }

  cancelItem() {
    this.isModalOpen = false;
    this.editingIndex = null;
  }

  get isReadOnly(): boolean {
    return this.order?.status === OrderStatus.Completed || this.order?.status === OrderStatus.Cancelled;
  }



  saveOrderWithStatus(status: OrderStatus) {
    const dto: OrderDto = {
      ...this.order,
      ...this.orderForm.value,
      status,
      orderItems: this.itemsFormArray.value
    };

    const saveAction = this.order?.id
      ? this.orderService.update(this.order.id, dto)
      : this.orderService.create(dto);

    saveAction.subscribe({
      next: savedOrder => {
        this.order = savedOrder;
        this.orderForm.patchValue(savedOrder);

        alert(`Order ${this.order?.id ? 'updated' : 'created'} successfully`);

        this.router.navigate(['/orders']);
      }
    });

  }

  onSave() {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }
    this.saveOrderWithStatus(this.orderForm.value.status);
  }

  onComplete() {
    this.saveOrderWithStatus(OrderStatus.Completed);
  }

  onCancel() {
    if (this.order?.id) {
      this.orderService.cancel(this.order.id)
        .subscribe(() => {
          console.log("Cancelled")
          this.router.navigate(['/orders']);
        });
    }
    else {
      this.router.navigate(['/orders']);
    }
  }

}
