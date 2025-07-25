using MassTransit.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services.Interfaces;
using services.Models;

namespace fragomen_order_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IService _service;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IService service, ILogger<OrderController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("get")]
        public IActionResult GetOrders()
        {
            try
            {
                _logger.LogInformation("Fetching all orders.");
                var orders = _service.GetOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid order ID.");
                }

                var order = _service.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the order with ID {OrderId}.", id);
                throw;
            }
        }

        [HttpPost("add-product")]
        public IActionResult AddProductToCart([FromBody] Order[] orderDetails)
        {
            try
            {
                if (orderDetails == null || orderDetails.Length == 0 || orderDetails.Any(o => o.ProductId <= 0 || o.StateId < 1))
                {
                    return BadRequest("Order details are invalid.");
                }

                var createdOrders = _service.CreateOrder(orderDetails.ToList());

                return Ok(createdOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                throw;
            }
        }

        [HttpPost("checkout/{id}")]
        public IActionResult CheckoutOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid order ID.");
                }

                var isCheckedOut = _service.CheckoutOrder(id);
                if (!isCheckedOut.Result)
                {
                    return NotFound($"Order with ID {id} not found or cannot be checked out.");
                }
                return Ok($"Order with ID {id} checked out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                throw;
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateOrder([FromBody] Order orderDetails)
        {
            if (orderDetails == null || orderDetails.ProductId <= 0 || orderDetails.StateId < 1)
            {
                return BadRequest("Order details are invalid.");
            }

            try
            {
                var updatedOrder = _service.UpdateOrder(orderDetails);
                if (updatedOrder == null)
                {
                    return NotFound($"Order with ID {orderDetails.Id} not found.");
                }
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order with ID {OrderId}.", orderDetails.Id);
                throw;
            }
        }

        [HttpDelete("remove/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var isDeleted = _service.DeleteOrder(id);
                if (!isDeleted.Result)
                {
                    return NotFound($"Order with ID {id} not found.");
                }
                return Ok($"Order with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the order with ID {OrderId}.", id);
                throw;
            }
        }
    }
}
