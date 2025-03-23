using Payphone.Domain.Entities;

namespace Payphone.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IReadOnlyList<Order>> GetAllAsync();
        Task<int> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
