using Microsoft.EntityFrameworkCore;
using Payphone.Domain.Entities;

namespace Payphone.Infrastructure.Persistence
{
    public class PayphoneDbContext : DbContext
    {
        public PayphoneDbContext(DbContextOptions<PayphoneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
