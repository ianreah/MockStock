﻿using System.Threading.Tasks;
using MockStock.Core;
using SignalR.Hubs;

namespace MockStock
{
	public class StockHub : Hub, IDisconnect
	{
		private static readonly ISubscriptionStore subscriptionStore = new InMemorySubscriptionStore();
		private readonly GroupSubscriptionManager subscriptionManager;

		public StockHub()
		{
			subscriptionManager = new GroupSubscriptionManager(
				new PriceFeedGenerator(),
				new PriceFeedSubscriber(p => Clients[p.Symbol].updatePrice(p)),
				subscriptionStore);
		}

		public void Subscribe(string symbol)
		{
			if (SymbolIsValid(symbol))
			{
				subscriptionManager.Subscribe(symbol, Context.ConnectionId);
				Groups.Add(Context.ConnectionId, symbol);
			}
		}

		public void Unsubscribe(string symbol)
		{
			subscriptionManager.Unsubscribe(symbol, Context.ConnectionId);
			Groups.Remove(Context.ConnectionId, symbol);
		}

		public Task Disconnect()
		{
			return Task.Factory.StartNew(() => subscriptionManager.UnsubscribeAll(Context.ConnectionId));
		}

		private static bool SymbolIsValid(string symbol)
		{
			return !string.IsNullOrWhiteSpace(symbol);
		}
	}
}