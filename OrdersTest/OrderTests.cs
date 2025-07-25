using AutoFixture;
using fragomen_order_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using services.Helpers;
using services.Interfaces;
using services.Models;

namespace OrdersTest
{
    [TestClass]
    public class OrderTests
    {
        private readonly Mock<IService> _service;   
        private readonly Mock<ILogger<OrderController>> _logger;
        private Fixture fixture; 
        private OrderController _controller;
        public OrderTests()
        {
            fixture = new Fixture();
            _service = new Mock<IService>();
            _logger = new Mock<ILogger<OrderController>>();
        }

        [TestMethod]
        public void CanGetOrder()
        {
            var orders = fixture.CreateMany<Order>(5).ToList();

            _service.Setup(repo => repo.GetOrders()).ReturnsAsync(orders);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.GetOrders();
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);
        }

        [TestMethod]
        public void CanGetOrderById()
        {
            var order = fixture.Create<Order>();
            _service.Setup(repo => repo.GetOrderById(order.Id)).ReturnsAsync(order);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.GetOrder(order.Id);
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);
        }

        [TestMethod]
        public void CanAddProductToCart()
        {
            var orders = fixture.CreateMany<Order>(5).ToList();
            _service.Setup(repo => repo.CreateOrder(orders)).ReturnsAsync(orders);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.AddProductToCart(orders.ToArray());
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);
        }

        [TestMethod]
        public void CanCheckoutOrder()
        {
            var orderId = fixture.Create<int>();
            _service.Setup(repo => repo.CheckoutOrder(orderId)).ReturnsAsync(true);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.CheckoutOrder(orderId);
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);
        }

        [TestMethod]
        public void CanDeleteOrder()
        {
            var orderId = fixture.Create<int>();
            _service.Setup(repo => repo.DeleteOrder(orderId)).ReturnsAsync(true);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.DeleteOrder(orderId);
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);
        }

        [TestMethod]
        public void CanUpdateOrder()
        {
            var order = fixture.Create<Order>();
            _service.Setup(repo => repo.UpdateOrder(order)).ReturnsAsync(order);
            _controller = new OrderController(_service.Object, _logger.Object);

            var result = _controller.UpdateOrder(order);
            var obj = result as ObjectResult;

            Assert.AreEqual(obj?.StatusCode, 200);  
        }
    }
}