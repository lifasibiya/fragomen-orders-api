using services.Interfaces;
using services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data;

namespace services.Helpers
{
    public class Repository : IRepository
    {
        private readonly OrdersDBContext _context;
        public Repository(OrdersDBContext context)
        {
            _context = context;
        }
        public Task<List<Order>?> CreateOrder(List<Order> orderDetails)
        {
            if (orderDetails == null || !orderDetails.Any())
            {
                return Task.FromResult<List<Order>?>(null);
            }

            foreach (var order in orderDetails)
            {
                order.StateId = (int)OrderStateEnum.Drafted;
                _context.Order.Add(order);
            }

            _context.SaveChanges();
            return Task.FromResult<List<Order>?>(orderDetails);
        }

        public Task<bool> DeleteOrder(int orderId)
        {
            var existingOrder = _context.Order.FirstOrDefault(o => o.Id == orderId);
            if (existingOrder == null)
            {
                return Task.FromResult<bool>(false);
            }

            if (existingOrder.StateId != (int)OrderStateEnum.Completed)
            {
                _context.Order.Remove(new Order { Id = orderId });
            } 
            else
            {
                existingOrder.IsDeleted = true;
                _context.Order.Update(existingOrder);
            }
            _context.SaveChanges();
            return Task.FromResult<bool>(true);
        }

        public Task<Order?> GetOrderById(int orderId)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
            return Task.FromResult<Order?>(order);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var orders = _context.Order.ToList();
            return await Task.FromResult<IEnumerable<Order>>(orders);
        }

        public Task<IEnumerable<StateChange>> GetStateChanges(int orderId)
        {
            var stateChanges = _context.StateChanges
                .Where(sc => sc.OrderId == orderId)
                .ToList();
            return Task.FromResult<IEnumerable<StateChange>>(stateChanges);
        }

        public Task<Order?> UpdateOrder(Order orderDetails)
        {
            var existingOrder = _context.Order.FirstOrDefault(o => o.Id == orderDetails.Id);
            if (existingOrder != null)
            {
                existingOrder.ProductId = orderDetails.ProductId;
                existingOrder.Description = orderDetails.Description;
                existingOrder.StateId = orderDetails.StateId;
                existingOrder.IsDeleted = orderDetails.IsDeleted;

                _context.Order.Update(existingOrder);
                _context.SaveChanges();
                return Task.FromResult<Order?>(existingOrder);
            }
            return Task.FromResult<Order?>(null);
        }

        public Task<StateChange?> UpdateStateChange(StateChange stateChange)
        {
            var existingChange = _context.StateChanges.FirstOrDefault(sc => sc.Id == stateChange.Id);
            if (existingChange != null)
            {
                existingChange.OrderId = stateChange.OrderId;
                existingChange.PrevState = stateChange.PrevState;
                existingChange.NewState = stateChange.NewState;
                existingChange.Timestamp = stateChange.Timestamp;

                _context.StateChanges.Update(existingChange);
                _context.SaveChanges();
                return Task.FromResult<StateChange?>(existingChange);
            }
            return Task.FromResult<StateChange?>(existingChange);
        }
    }
}
