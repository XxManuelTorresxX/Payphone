namespace Payphone.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }     
        public string CustomerId { get; private set; }        
        public decimal TotalAmount { get; private set; }        
        public OrderStatus Status { get; private set; }        
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
                
        private Order() { }
                
        public Order(string customerId, decimal totalAmount)
        {        
            if (totalAmount <= 0)
                throw new ArgumentException("El monto total debe ser mayor que cero.");

            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
            TotalAmount = totalAmount;
            Status = OrderStatus.Pendiente;  // Por defecto, inicia en Pendiente
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void UpdateTotalAmount(decimal newAmount)
        {
            if (newAmount <= 0)
                throw new ArgumentException("El monto total debe ser mayor que cero.");

            TotalAmount = newAmount;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void ChangeStatus(OrderStatus newStatus)
        {
         
            ValidateStatusTransition(Status, newStatus);
         
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
                
        private void ValidateStatusTransition(OrderStatus current, OrderStatus next)
        {
            if (current == OrderStatus.Pendiente && next == OrderStatus.Entregado)
                throw new InvalidOperationException("No se puede pasar de Pendiente a Entregado sin Procesar y Enviar.");

            if (current == OrderStatus.Pendiente && next == OrderStatus.Enviado)
                throw new InvalidOperationException("No se puede pasar de Pendiente a Enviado sin pasar por Procesando.");

            if (current == OrderStatus.Procesando && next == OrderStatus.Entregado)
                throw new InvalidOperationException("No se puede pasar de Procesando a Entregado sin pasar por Enviado.");
        
        }
    }
}
