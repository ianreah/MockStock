using System;

namespace MockStock.Core
{
	public interface IPriceFeedGenerator
	{
		IObservable<StockPrice> Generate(string symbol);
	}
}