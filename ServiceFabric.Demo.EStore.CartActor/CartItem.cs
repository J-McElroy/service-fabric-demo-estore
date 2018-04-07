using System;
using System.Runtime.Serialization;

namespace ServiceFabric.Demo.EStore.CartActor
{
    [DataContract]
    class CartItem
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
