using System;

namespace ServiceFabric.Demo.EStore.WebAPI.Model
{
    public class ApiProduct
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
