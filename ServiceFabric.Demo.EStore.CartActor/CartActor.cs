using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceFabric.Demo.EStore.CartActor.Interfaces;

namespace ServiceFabric.Demo.EStore.CartActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    public class CartActor : Actor, ICartActor
    {
        const string CartItemsStateName = "CartItems";
        private List<CartItem> cartItems = new List<CartItem>();

        /// <summary>
        /// Initializes a new instance of CartActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public CartActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task Add(Guid productId, int quantity)
        {
            var existingItem = cartItems.FirstOrDefault(el => el.Id == productId);

            if (existingItem != null)
            {
                existingItem.Count += quantity;
            }
            else
            {
                var newItem = new CartItem() { Id = productId, Count = quantity };
                cartItems.Add(newItem);
            }

            return StateManager.AddOrUpdateStateAsync(CartItemsStateName, cartItems, (s, obj) => cartItems);
        }

        public Task Clear()
        {
            return StateManager.RemoveStateAsync(CartItemsStateName);
        }

        public Task<Dictionary<Guid, int>> GetItems()
        {
            return Task.FromResult(cartItems.ToDictionary(el => el.Id, el => el.Count));
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            var state = await StateManager.TryGetStateAsync<List<CartItem>>(CartItemsStateName);

            if(state.HasValue)
            {
                cartItems = state.Value;
            }
        }

        protected override async Task OnDeactivateAsync()
        {

        }
    }
}
