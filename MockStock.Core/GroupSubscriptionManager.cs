using System.Linq;

namespace MockStock.Core
{
	public class GroupSubscriptionManager
	{
		private readonly IPriceFeedGenerator priceFeedGenerator;
		private readonly ISubscriptionStore subscriptionStore;

		// Pessimistic locking to make Subscribe and Unsubscribe atomic & mutually exclusive.
		// In the real world it should just need to manage concurrent subscription and
		// unsubscription to the same symbol?
		private static readonly object lockObject = new object();

		public GroupSubscriptionManager(IPriceFeedGenerator priceFeedGenerator, ISubscriptionStore subscriptionStore)
		{
			this.priceFeedGenerator = priceFeedGenerator;
			this.subscriptionStore = subscriptionStore;
		}

		public void Subscribe(string symbol, string clientId)
		{
			lock (lockObject)
			{
				if (!subscriptionStore.Exists(symbol))
				{
					subscriptionStore.AddSubscription(symbol, priceFeedGenerator.Generate(symbol));
				}

				subscriptionStore.AddSubscriber(symbol, clientId);
			}
		}

		public void Unsubscribe(string symbol, string clientId)
		{
			lock (lockObject)
			{
				subscriptionStore.RemoveSubscriber(symbol, clientId);

				if (!subscriptionStore.AnySubscribers(symbol))
				{
					subscriptionStore.RemoveSubscription(symbol).Dispose();
				}
			}
		}

		public void UsubscribeAll(string clientId)
		{
			foreach (var symbol in subscriptionStore.ClientSubscriptions(clientId).ToList())
			{
				Unsubscribe(symbol, clientId);
			}
		}
	}
}