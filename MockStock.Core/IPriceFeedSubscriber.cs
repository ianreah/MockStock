using System;

namespace MockStock.Core
{
	public interface IPriceFeedSubscriber
	{
		IDisposable Subscribe(IObservable<StockPrice> priceFeed);
	}
}