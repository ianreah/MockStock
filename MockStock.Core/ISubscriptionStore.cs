using System;

namespace MockStock.Core
{
	public interface ISubscriptionStore
	{
		void AddSubscriber(string symbol, string clientId);
		void RemoveSubscriber(string symbol, string clientId);

		void AddSubscription(string symbol, IDisposable subscription);
		IDisposable RemoveSubscription(string symbol);

		bool Exists(string symbol);
		bool AnySubscribers(string symbol);
	}
}