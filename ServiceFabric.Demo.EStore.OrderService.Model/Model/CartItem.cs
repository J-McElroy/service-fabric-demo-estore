using System;

namespace ServiceFabric.Demo.EStore.OrderService.Model.Model
{
    public class CartItem
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal TotalPrice => ProductPrice * Quantity;
    }
}
