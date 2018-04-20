using System;
using System.Runtime.Serialization;

namespace ServiceFabric.Demo.EStore.OrderService.Model.Exceptions
{
    [Serializable]
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ProductNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
