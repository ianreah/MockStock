using System;
using MockStock.Core;

namespace MockStock.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var priceFeedGenerator = new PriceFeedGenerator();
			var priceFeedSubscriber = new PriceFeedSubscriber(p =>
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

			var priceFeed = priceFeedGenerator.Generate("XYZ");
			var subscription = priceFeedSubscriber.Subscribe(priceFeed);

			System.Console.ReadLine();
			subscription.Dispose();

			System.Console.ReadLine();
		}
	}
}