using Payphone.Application.Interfaces;
using Payphone.Domain.Entities;
using Payphone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Payphone.Infrastructure.Repositories
{
    public class OrderStatusHistoryRepository : IOrderStatusHistoryRepository
    {
        private readonly PayphoneDbContext _context;

        public OrderStatusHistoryRepository(PayphoneDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(OrderStatusHistory history)
        {
            _context.OrderStatusHistories.Add(history);
            await _context.SaveChangesAsync();
            return history.Id;
        }

        public async Task<IReadOnlyList<OrderStatusHistory>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderStatusHistories
                                 .Where(h => h.OrderId == orderId)
                                 .OrderBy(h => h.ChangedAt)
                                 .ToListAsync();
        }
    }
}
