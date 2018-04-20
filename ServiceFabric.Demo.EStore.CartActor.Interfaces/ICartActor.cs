using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ServiceFabric.Demo.EStore.CartActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ICartActor : IActor
    {
        Task Add(Guid productId, int quantity);

        Task<Dictionary<Guid, int>> GetItems();

        Task Clear();
    }
}
