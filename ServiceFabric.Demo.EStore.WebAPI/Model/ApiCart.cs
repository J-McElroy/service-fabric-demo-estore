using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.WebAPI.Model
{
    public class ApiCart
    {
        public List<ApiCartItem> Items { get; set; }

        public decimal Total => Items?.Select(el => el.TotalPrice).DefaultIfEmpty().Sum() ?? 0M;
    }
}
