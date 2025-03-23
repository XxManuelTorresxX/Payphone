using Payphone.Application.Interfaces;
using Payphone.Domain.Entities;
using Payphone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Payphone.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PayphoneDbContext _context;

        public OrderRepository(PayphoneDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
