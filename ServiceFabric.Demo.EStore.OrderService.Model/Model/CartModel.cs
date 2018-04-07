using System.Collections.Generic;
using System.Linq;

namespace ServiceFabric.Demo.EStore.OrderService.Model.Model
{
    public class CartModel
    {
        public List<CartItem> Items { get; set; }

        public decimal Total => Items?.Select(el => el.TotalPrice).DefaultIfEmpty().Sum() ?? 0M;
    }
}
