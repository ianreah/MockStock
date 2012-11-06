using System;

namespace MockStock.Core
{
	public interface IPriceFeedGenerator
	{
		IDisposable Generate(string symbol);
	}
}