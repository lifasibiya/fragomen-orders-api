using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int ProductId { get; set; }
        public int StateId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class StateChange
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? PrevState { get; set; }
        public int NewState { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class OrderState
    {
        public int Id { get; set; }
        public string State { get; set; } = string.Empty;
    }

    public enum OrderStateEnum
    {
        Drafted = 1,
        Submitted = 2,
        Completed = 3
    }

    public class OrderStateChangeEvent
    {
        public int OrderId { get; set; }
        public int? PrevState { get; set; }
        public int NewState { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Barcode { get; set; }
    }
}
