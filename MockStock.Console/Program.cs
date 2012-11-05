using System;
using System.Reactive.Linq;
using MockStock.Core;

namespace MockStock.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var random = new Random();
			IObservable<StockPrice> priceFeed = Observable.Generate(new StockPrice("XYZ", random.NextDouble() * 100), // starting value random price 0 - 100
																 p => true,                                        // condition - keep going forever!
																 p => p.NextPrice(random.NextDouble() - 0.5),      // update the price with a random fluctuation between -0.5 and +0.5
																 p => p,                                           // selector - just return the price object
																 p => TimeSpan.FromSeconds(random.NextDouble() + 0.5)); // update randomly every 0.5 - 1.5 seconds

			var subscription = priceFeed.Subscribe(p =>
				                                    {
					                                    System.Console.Clear();
					                                    System.Console.Write("{0}: {1:N2} ", p.Symbol, p.Price);
					                                    if (p.Change < 0)
					                                    {
						                                    System.Console.ForegroundColor = ConsoleColor.Red;
						                                    System.Console.Write("{0:N2}", p.Change);
					                                    }
					                                    else
					                                    {
						                                    System.Console.ForegroundColor = ConsoleColor.DarkGreen;
						                                    System.Console.Write("+{0:N2}", p.Change);
					                                    }
					                                    System.Console.ResetColor();
				                                    });

			System.Console.ReadLine();
			subscription.Dispose();

			System.Console.ReadLine();
		}
	}
}
