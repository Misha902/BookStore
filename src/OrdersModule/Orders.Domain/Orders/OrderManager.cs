using Acme.BookStore;
using Acme.BookStore.Books;
using Orders.Domain.Shared.OrderItems;
using Orders.Domain.Shared.Orders;
using Polly;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Orders.Domain.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly BookManager _bookManager;

        public OrderManager(IOrderRepository orderRepository, BookManager bookManager)
        {
            _orderRepository = orderRepository;
            _bookManager = bookManager;
        }


        public async Task<Order> GetWithItemAsync(Guid id)
        {
            var order = await _orderRepository.GetWithItemsAsync(id);

            if (order.Status == OrderStatus.Completed)
            {

            }
            else 
            {

            }


            return order;
        }


        public async Task<List<Order>> GetListWithItemsAsync(int skipCount, int maxResultCount, string sorting)
        {
            return await _orderRepository.GetListWithItemsAsync(skipCount, maxResultCount, sorting);
        }

        public async Task<long> GetCountAsync()
        {
            return await _orderRepository.GetCountAsync();
        }

        public async Task<Order> CreateAsync(Order createdOrder)
        {
            var order = new Order(GuidGenerator.Create(), createdOrder.Address, createdOrder.Status);

            foreach (var item in createdOrder.OrderItems)
            {
                var book = await _bookManager.GetAsync(item.BookId);

                if (order.Status == OrderStatus.Completed)
                {
                    await CheckQuantity(book, item.Quantity);
                }
            }

            foreach (var item in createdOrder.OrderItems)
            {
                var book = await _bookManager.GetAsync(item.BookId);
                order.AddItem(book, item.Quantity);
            }

            return await _orderRepository.InsertAsync(order, autoSave: true);
        }


        public async Task<Order> UpdateAsync(Order updatedOrder)
        {
            var order = await GetWithItemAsync(updatedOrder.Id);

            if (updatedOrder.Status == OrderStatus.Completed)
            {
                foreach (var item in updatedOrder.OrderItems)
                {
                    var book = await _bookManager.GetAsync(item.BookId);

                    if (book.Quantity < item.Quantity)
                    {
                        throw new BusinessException(BookStoreDomainErrorCodes.DontEnoughBooks)
                            .WithData("Qty", item.Quantity)
                            .WithData("BookQty", book.Quantity);
                    }
                }
            }

            order.ChangeAddress(updatedOrder.Address);
            order.ChangeStatus(updatedOrder.Status);
            order.TotalPrice = 0;
            order.ClearItems();

            foreach (var item in updatedOrder.OrderItems)
            {
                var book = await _bookManager.GetAsync(item.BookId);
                if (order.Status == OrderStatus.Completed)
                {
                    await _bookManager.DecreaseQuantity(book, item.Quantity);
                }

                order.AddItem(book, item.Quantity);
            }
            
            var savedOrder = await _orderRepository.UpdateAsync(order);

            return savedOrder;
        }


        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<Order> CancelAsync(Guid id)
        {
            var order = await _orderRepository.GetWithItemsAsync(id);
            order.ChangeStatus(OrderStatus.Cancelled);

            return await _orderRepository.UpdateAsync(order);
        }

        private async Task CheckQuantity(Book book, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new BusinessException("quantity should be greater than zero");

            var diff = book.Quantity - newQuantity;

            if (diff >= 0)
            {
                await _bookManager.DecreaseQuantity(book, newQuantity);
            }
            else
            {
                throw new BusinessException(BookStoreDomainErrorCodes.DontEnoughBooks)
                    .WithData("Qty", newQuantity)
                    .WithData("BookQty", book.Quantity);
            }
        }

    }
}
