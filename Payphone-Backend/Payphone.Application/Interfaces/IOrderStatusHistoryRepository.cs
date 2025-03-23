using Payphone.Domain.Entities;

namespace Payphone.Application.Interfaces
{
    public interface IOrderStatusHistoryRepository
    {
        Task<int> AddAsync(OrderStatusHistory history);
        Task<IReadOnlyList<OrderStatusHistory>> GetByOrderIdAsync(int orderId);
    }
}
