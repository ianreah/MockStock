namespace MockStock.Core
{
	public class StockPrice
	{
		private readonly string symbol;

		public StockPrice(string symbol, double startingPrice)
		{
			this.symbol = symbol;

			Price = startingPrice;
		}

		public double Price { get; private set; }
		public double Change { get; private set; }

		public string Symbol
		{
			get { return symbol; }
		}

		public StockPrice NextPrice(double change)
		{
			// Ignore any change that makes the price go negative
			if (Price + change > 0)
			{
				Change = change;
				Price += Change;
			}

			return this;
		}
	}
}