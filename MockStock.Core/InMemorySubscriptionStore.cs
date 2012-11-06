using System;
using System.Collections.Generic;
using System.Linq;

namespace MockStock.Core
{
	public class InMemorySubscriptionStore : ISubscriptionStore
	{
		private readonly Dictionary<string, Tuple<IDisposable, Dictionary<string, object>>> subscriptions = new Dictionary<string, Tuple<IDisposable, Dictionary<string, object>>>();

		public void AddSubscriber(string symbol, string clientId)
		{
			ThrowIfSubscriptionDoesNotExist(symbol);
			subscriptions[symbol].Item2[clientId] = null;
		}

		public void RemoveSubscriber(string symbol, string clientId)
		{
			ThrowIfSubscriptionDoesNotExist(symbol);
			subscriptions[symbol].Item2.Remove(clientId);
		}

		public void AddSubscription(string symbol, IDisposable subscription)
		{
			ThrowIfSubscriptionAlreadyExists(symbol);
			subscriptions[symbol] = new Tuple<IDisposable, Dictionary<string, object>>(subscription, new Dictionary<string, object>());
		}

		public IDisposable RemoveSubscription(string symbol)
		{
			ThrowIfSubscriptionDoesNotExist(symbol);

			var subscription = subscriptions[symbol].Item1;
			subscriptions.Remove(symbol);
			return subscription;
		}

		public bool Exists(string symbol)
		{
			return subscriptions.ContainsKey(symbol);
		}

		public bool AnySubscribers(string symbol)
		{
			return Exists(symbol) && subscriptions[symbol].Item2.Any();
		}

		private void ThrowIfSubscriptionAlreadyExists(string symbol)
		{
			if (Exists(symbol))
			{
				throw new InvalidOperationException();
			}
		}

		private void ThrowIfSubscriptionDoesNotExist(string symbol)
		{
			if (!Exists(symbol))
			{
				throw new InvalidOperationException();
			}
		}
	}
}