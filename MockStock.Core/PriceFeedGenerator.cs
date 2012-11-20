using System;
using System.Reactive.Linq;

namespace MockStock.Core
{
	public class PriceFeedGenerator : IPriceFeedGenerator
	{
		public IObservable<StockPrice> Generate(string symbol)
		{
			var random = new Random();
			return Observable.Generate(new StockPrice(symbol, random.NextDouble() * 100), // starting value random price 0 - 100
			                                     p => true,                                        // condition - keep going forever!
			                                     p => p.NextPrice(random.NextDouble() - 0.5),      // update the price with a random fluctuation between -0.5 and +0.5
			                                     p => p,                                           // selector - just return the price object
			                                     p => TimeSpan.FromSeconds(random.NextDouble() + 0.5)); // update randomly every 0.5 - 1.5 seconds
		}
	}
}