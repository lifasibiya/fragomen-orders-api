using services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.Interfaces
{
    public interface IRepository
    {
        public Task<List<Order>?> CreateOrder(List<Order> orderDetails);
        public Task<bool> DeleteOrder(int orderId);
        public Task<IEnumerable<Order>> GetOrders();
        public Task<Order?> UpdateOrder(Order orderDetails);
        public Task<Order?> GetOrderById(int orderId);
        public Task<IEnumerable<StateChange>> GetStateChanges(int orderId);
        public Task<StateChange?> UpdateStateChange(StateChange stateChange);
    }
}
