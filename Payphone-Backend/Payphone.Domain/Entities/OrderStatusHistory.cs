namespace Payphone.Domain.Entities
{
    public class OrderStatusHistory
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public OrderStatus PreviousStatus { get; private set; }
        public OrderStatus NewStatus { get; private set; }

        public DateTime ChangedAt { get; private set; }

        // Constructor privado para EF
        private OrderStatusHistory() { }
        
        public OrderStatusHistory(int orderId, OrderStatus previousStatus, OrderStatus newStatus)
        {
            OrderId = orderId;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            ChangedAt = DateTime.UtcNow;
        }
    }
}
