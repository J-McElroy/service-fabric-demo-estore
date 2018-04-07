using System;

namespace ServiceFabric.Demo.EStore.WebAPI.Model
{
    public class ApiCartAddRequest
    {
        public Guid ProductId { get; set; }
          
        public int Quantity { get; set; }
    }
}
