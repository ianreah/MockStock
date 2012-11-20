using System;

namespace MockStock.Core
{
	public class PriceFeedSubscriber : IPriceFeedSubscriber
	{
		private readonly Action<StockPrice> action;

		public PriceFeedSubscriber(Action<StockPrice> action)
		{
			this.action = action;
		}

		public IDisposable Subscribe(IObservable<StockPrice> priceFeed)
		{
			return priceFeed.Subscribe(action);
		}
	}
}