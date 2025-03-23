using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payphone.Application.Interfaces;
using Payphone.Domain.Entities;

namespace Payphone.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor del controlador de pedidos.
        /// </summary>
        /// <param name="orderRepository">Implementación de IOrderRepository para acceder a los pedidos.</param>
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Obtiene la lista de todos los pedidos.
        /// </summary>
        /// <returns>Una lista de objetos <see cref="Order"/>.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Obtiene un pedido por su identificador.
        /// </summary>
        /// <param name="id">El identificador único del pedido.</param>
        /// <returns>El objeto <see cref="Order"/> con el ID especificado, o un código 404 si no se encuentra.</returns>
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound($"No se encontró el pedido con id {id}.");

            return Ok(order);
        }

        /// <summary>
        /// Crea un nuevo pedido.
        /// </summary>
        /// <param name="dto">Información necesaria para crear el pedido.</param>
        /// <returns>El pedido creado con código 201 (Created). Retorna 400 si el monto total es inválido.</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (dto.TotalAmount <= 0)
                return BadRequest("El monto total debe ser mayor que cero.");

            var order = new Order(dto.CustomerId, dto.TotalAmount);
            var orderId = await _orderRepository.AddAsync(order);

            return CreatedAtAction(nameof(GetById), new { id = orderId }, order);
        }

        /// <summary>
        /// Actualiza un pedido existente, permitiendo modificar el monto total y/o el estado.
        /// </summary>
        /// <param name="id">El identificador del pedido a actualizar.</param>
        /// <param name="dto">Los datos de actualización (monto y estado).</param>
        /// <returns>
        /// 204 (NoContent) si la actualización se realiza con éxito.
        /// 404 si no se encuentra el pedido.
        /// 400 si la transición de estado es inválida o el monto no es válido.
        /// </returns>
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] OrderUpdateDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound($"No se encontró el pedido con id {id}.");

            if (dto.TotalAmount.HasValue)
                order.UpdateTotalAmount(dto.TotalAmount.Value);

            if (dto.NewStatus.HasValue)
            {
                try
                {
                    order.ChangeStatus(dto.NewStatus.Value);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest($"Transición de estado inválida: {ex.Message}");
                }
            }

            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }

        /// <summary>
        /// Elimina un pedido existente.
        /// </summary>
        /// <param name="id">El identificador del pedido a eliminar.</param>
        /// <returns>
        /// 204 (NoContent) si se elimina con éxito.
        /// 404 si no se encuentra el pedido.
        /// </returns>
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _orderRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el pedido con id {id}.");

            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    /// <summary>
    /// DTO para la creación de un nuevo pedido.
    /// </summary>
    /// <param name="CustomerId">El identificador del cliente que realiza el pedido.</param>
    /// <param name="TotalAmount">El monto total del pedido (debe ser mayor que cero).</param>
    public record OrderCreateDto(string CustomerId, decimal TotalAmount);

    /// <summary>
    /// DTO para la actualización de un pedido existente.
    /// </summary>
    /// <param name="TotalAmount">Nuevo monto total (opcional, debe ser mayor que cero).</param>
    /// <param name="NewStatus">Nuevo estado del pedido (opcional).</param>
    public record OrderUpdateDto(decimal? TotalAmount, OrderStatus? NewStatus);
}
