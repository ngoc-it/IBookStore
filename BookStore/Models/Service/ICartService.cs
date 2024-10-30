using BookStore.Models.Data;
using BookStore.Models.Model;
using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Service
{
    public interface ICartService : IBaseService<Cart>
    {
        Task CreateNewOrder(int userId, CartConfirmModel model);
        Task<PagingModel<OrderViewModel>> GetPagingOrder(OrderStatus status, int? pageIndex, int? userId = null);
        Task<OrderViewModel> GetOrderDetail(int orderId);
        Task UpdateOrderStatus(int orderId, OrderStatus status);
        Task CancelOrder(int orderId, string reason);
    }
}
