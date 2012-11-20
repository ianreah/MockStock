using System.Linq;

namespace MockStock.Core
{
	public class GroupSubscriptionManager
	{
		private readonly IPriceFeedGenerator priceFeedGenerator;
		private readonly IPriceFeedSubscriber priceFeedSubscriber;
		private readonly ISubscriptionStore subscriptionStore;

		// Pessimistic locking to make Subscribe and Unsubscribe atomic & mutually exclusive.
		// In the real world it should just need to manage concurrent subscription and
		// unsubscription to the same symbol?
		private static readonly object lockObject = new object();

		public GroupSubscriptionManager(IPriceFeedGenerator priceFeedGenerator, IPriceFeedSubscriber priceFeedSubscriber, ISubscriptionStore subscriptionStore)
		{
			this.priceFeedGenerator = priceFeedGenerator;
			this.priceFeedSubscriber = priceFeedSubscriber;
			this.subscriptionStore = subscriptionStore;
		}

		public void Subscribe(string symbol, string clientId)
		{
			lock (lockObject)
			{
				if (!subscriptionStore.Exists(symbol))
				{
					var priceFeed = priceFeedGenerator.Generate(symbol);
					var subscription = priceFeedSubscriber.Subscribe(priceFeed);

					subscriptionStore.AddSubscription(symbol, subscription);
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

		public void UnsubscribeAll(string clientId)
		{
			foreach (var symbol in subscriptionStore.ClientSubscriptions(clientId).ToList())
			{
				Unsubscribe(symbol, clientId);
			}
		}
	}
}