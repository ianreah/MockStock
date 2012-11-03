using SignalR.Hubs;

namespace MockStock
{
	public class StockHub : Hub
	{
		public void Subscribe(string symbol)
		{
			// Send data just to the calling client...
            Caller.updatePrice("Server Notification: I got your subscription");

			// Broadcast data to all clients...
            Clients.updatePrice(symbol);
		}
	}
}