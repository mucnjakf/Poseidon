namespace Poseidon.Server.Hubs;

public class EventHub : Hub
{
    public async Task SendEventAsync(EventModel eventModel)
    {
        await Clients.All.SendAsync("ReceiveEvent", eventModel);
    }
}