﻿using MockStock.Core;
using SignalR.Hubs;

namespace MockStock
{
	public class StockHub : Hub
	{
		private static readonly ISubscriptionStore subscriptionStore = new InMemorySubscriptionStore();
		private readonly GroupSubscriptionManager subscriptionManager;

		public StockHub()
		{
			subscriptionManager = new GroupSubscriptionManager(
				new PriceFeedGenerator(p => Clients[p.Symbol].updatePrice(p)),
				subscriptionStore);
		}

		public void Subscribe(string symbol)
		{
			subscriptionManager.Subscribe(symbol, Context.ConnectionId);
			Groups.Add(Context.ConnectionId, symbol);
		}

		public void Unsubscribe(string symbol)
		{
			subscriptionManager.Unsubscribe(symbol, Context.ConnectionId);
			Groups.Remove(Context.ConnectionId, symbol);
		}
	}
}