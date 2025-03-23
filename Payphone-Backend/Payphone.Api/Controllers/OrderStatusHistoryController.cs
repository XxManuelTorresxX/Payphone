using Microsoft.AspNetCore.Mvc;
using Payphone.Application.Interfaces;
using Payphone.Domain.Entities;

namespace Payphone.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar el historial de estados de los pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusHistoryController : ControllerBase
    {
        private readonly IOrderStatusHistoryRepository _historyRepository;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor del controlador de historial de estados de pedidos.
        /// </summary>
        /// <param name="historyRepository">Repositorio para manejar el historial de estados.</param>
        /// <param name="orderRepository">Repositorio para manejar los pedidos.</param>
        public OrderStatusHistoryController(
            IOrderStatusHistoryRepository historyRepository,
            IOrderRepository orderRepository)
        {
            _historyRepository = historyRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Obtiene el historial de cambios de estado para un pedido específico.
        /// </summary>
        /// <param name="orderId">El identificador único del pedido.</param>
        /// <returns>
        /// Una lista de <see cref="OrderStatusHistory"/> correspondiente al pedido especificado.
        /// Retorna 404 si el pedido no existe.
        /// </returns>
        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<IEnumerable<OrderStatusHistory>>> GetByOrderId(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return NotFound($"No se encontró el pedido con id {orderId}.");

            var history = await _historyRepository.GetByOrderIdAsync(orderId);
            return Ok(history);
        }

        /// <summary>
        /// Crea un nuevo registro en el historial de estados de un pedido.
        /// </summary>
        /// <param name="dto">Los datos necesarios para crear el registro de historial.</param>
        /// <returns>
        /// 201 (Created) con el historial creado y la ruta para obtenerlo.
        /// 404 si no se encuentra el pedido.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderStatusHistoryCreateDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order == null)
                return NotFound($"No se encontró el pedido con id {dto.OrderId}.");

            var history = new OrderStatusHistory(
                dto.OrderId,
                dto.PreviousStatus,
                dto.NewStatus
            );

            var newId = await _historyRepository.AddAsync(history);
            return CreatedAtAction(nameof(GetByOrderId), new { orderId = dto.OrderId }, history);
        }
    }

    /// <summary>
    /// DTO para crear un nuevo registro de historial de estado de un pedido.
    /// </summary>
    /// <param name="OrderId">El identificador único del pedido al que pertenece este historial.</param>
    /// <param name="PreviousStatus">El estado anterior del pedido.</param>
    /// <param name="NewStatus">El nuevo estado asignado al pedido.</param>
    public record OrderStatusHistoryCreateDto(int OrderId, OrderStatus PreviousStatus, OrderStatus NewStatus);
}
