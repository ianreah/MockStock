namespace MockStock.Core
{
	public class GroupSubscriptionManager
	{
		private readonly IPriceFeedGenerator priceFeedGenerator;
		private readonly ISubscriptionStore subscriptionStore;

		public GroupSubscriptionManager(IPriceFeedGenerator priceFeedGenerator, ISubscriptionStore subscriptionStore)
		{
			this.priceFeedGenerator = priceFeedGenerator;
			this.subscriptionStore = subscriptionStore;
		}

		public void Subscribe(string symbol, string clientId)
		{
			if(!subscriptionStore.Exists(symbol))
			{
				subscriptionStore.AddSubscription(symbol, priceFeedGenerator.Generate(symbol));
			}

			subscriptionStore.AddSubscriber(symbol, clientId);
		}

		public void Unsubscribe(string symbol, string clientId)
		{
			subscriptionStore.RemoveSubscriber(symbol, clientId);

			if(!subscriptionStore.AnySubscribers(symbol))
			{
				subscriptionStore.RemoveSubscription(symbol).Dispose();
			}
		}
	}
}