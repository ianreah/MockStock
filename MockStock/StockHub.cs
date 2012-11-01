using SignalR.Hubs;

namespace MockStock
{
	public class StockHub : Hub
	{
		public void Send(string message)
		{
			// Send data just to the calling client...
			Caller.addMessage("Server Notification: I got your message");

			// Broadcast data to all clients...
			Clients.addMessage(message);
		}
	}
}