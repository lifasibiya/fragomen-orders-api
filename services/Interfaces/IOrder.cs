using services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.Interfaces
{

    public interface IOrder
    {
        public Task<int> CreateOrder(string title, string description, int stateId);
        public Task<bool> UpdateOrder(int orderId, string title, string description, int stateId);
        public Task<bool> DeleteOrder(int orderId);
        public Task<IEnumerable<Order>> GetOrders();
        public Task<Order?> GetOrderById(int orderId);
        public Task<IEnumerable<StateChange>> GetStateChanges(int orderId);
    }
}
