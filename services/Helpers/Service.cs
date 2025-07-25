using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using services.Interfaces;
using services.Models;

namespace services.Helpers
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IPublishEndpoint _publishEndpoint;
        public Service(IRepository repository, IMemoryCache cache, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _cache = cache;
            _publishEndpoint = publishEndpoint;
        }

        public Task<bool> CheckoutOrder(int orderId)
        {
            if (orderId.Equals(0)) return Task.FromResult<bool>(false);

            var order = _repository.GetOrderById(orderId).Result;
            if (order == null) return Task.FromResult<bool>(false);

            if (order.StateId != (int)OrderStateEnum.Completed || order.StateId != (int)OrderStateEnum.Submitted)
            {
                order.StateId = (int)OrderStateEnum.Submitted;
                var updatedOrder = _repository.UpdateOrder(order).Result;

                if (updatedOrder != null)
                {
                    UpdateOrderStateChange(new OrderStateChangeEvent
                    {
                        OrderId = updatedOrder.Id,
                        PrevState = order.StateId,
                        NewState = (int)OrderStateEnum.Submitted,
                        Timestamp = DateTime.UtcNow
                    }).ConfigureAwait(false);
                }
            }

            return Task.FromResult<bool>(true);
        }

        public Task<bool> CompleteOrder(int orderId)
        {
            if (orderId.Equals(0)) return Task.FromResult<bool>(false);

            var order = _repository.GetOrderById(orderId).Result;
            if (order == null) return Task.FromResult<bool>(false);

            if (order.StateId != (int)OrderStateEnum.Completed)
            {
                order.StateId = (int)OrderStateEnum.Completed;
                var updatedOrder = _repository.UpdateOrder(order).Result;

                if (updatedOrder != null)
                {
                    UpdateOrderStateChange(new OrderStateChangeEvent
                    {
                        OrderId = updatedOrder.Id,
                        PrevState = order.StateId,
                        NewState = (int)OrderStateEnum.Completed,
                        Timestamp = DateTime.UtcNow
                    }).ConfigureAwait(false);
                }
            }

            return Task.FromResult<bool>(true);
        }

        public Task<List<Order>?> CreateOrder(List<Order> orderDetails)
        {
            if (orderDetails.Any(o => o.ProductId <= 0)) return Task.FromResult<List<Order>?>(null);

            Task<List<Order>?> createdOrder = _repository.CreateOrder(orderDetails);
            if (createdOrder.Result != null)
            {
                foreach (var order in orderDetails)
                {
                    UpdateOrderStateChange(new OrderStateChangeEvent
                    {
                        OrderId = order.Id,
                        PrevState = null, 
                        NewState = (int)OrderStateEnum.Drafted,
                        Timestamp = DateTime.UtcNow
                    }).ConfigureAwait(false);
                }
            }   

            return createdOrder;
        }

        public Task<bool> DeleteOrder(int orderId)
        {
            if (orderId.Equals(0)) return Task.FromResult<bool>(false);
            return _repository.DeleteOrder(orderId);
        }

        public Task<Order?> GetOrderById(int orderId)
        {
            if (orderId.Equals(0)) return Task.FromResult<Order?>(null);
            return _repository.GetOrderById(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var cacheKey = "GetOrdersCache";
            IEnumerable<Order> cachedOrders;
            if (!_cache.TryGetValue(cacheKey, out cachedOrders))
            {
                cachedOrders = await _repository.GetOrders();

                if (cachedOrders != null)
                {
                    _cache.Set(cacheKey, cachedOrders, TimeSpan.FromMinutes(2)); 
                }
            }

            return cachedOrders ?? Enumerable.Empty<Order>(); 
        }

        public Task<IEnumerable<StateChange>> GetStateChanges(int orderId)
        {
            return _repository.GetStateChanges(orderId);
        }

        public Task<Order?> UpdateOrder(Order orderDetails)
        {
            if (orderDetails.Id.Equals(0)) return Task.FromResult<Order?>(null);

            if (orderDetails.StateId != (int) OrderStateEnum.Completed)
            {
                Task<Order?> updatedOrder = _repository.UpdateOrder(orderDetails);
                if (updatedOrder.Result != null)
                {
                    UpdateOrderStateChange(new OrderStateChangeEvent
                    {
                        OrderId = updatedOrder.Result.Id,
                        PrevState = updatedOrder.Result.StateId,
                        NewState = orderDetails.StateId,
                        Timestamp = DateTime.UtcNow
                    }).ConfigureAwait(false);
                }
            }

            return Task.FromResult<Order?>(null);
        }

        public async Task UpdateOrderStateChange(OrderStateChangeEvent orderStateChangeEvent)
        {
            if (orderStateChangeEvent == null) return;

            await _publishEndpoint.Publish(orderStateChangeEvent);
        }
    }
}
