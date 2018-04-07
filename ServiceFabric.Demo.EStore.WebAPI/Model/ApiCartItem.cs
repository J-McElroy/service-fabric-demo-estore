using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.WebAPI.Model
{
    public class ApiCartItem
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal TotalPrice => ProductPrice * Quantity;
    }
}
