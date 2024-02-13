namespace Poseidon.Client.Services;

public class GisService
{
    public event Func<Task> OpenEventDetailsModal;

    public EventModel EventModel { get; set; }

    public event Func<Task> NotifyLatLng;

    public string LatLon { get; set; } = "LATLNG(0, 0)";

    [JSInvokable("OpenEventDetailsModalAsync")]
    public async Task OpenEventDetailsModalAsync(object eventJson)
    {
        EventModel = JsonSerializer.Deserialize<EventModel>(eventJson.ToString()!,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (OpenEventDetailsModal is not null)
        {
            await OpenEventDetailsModal?.Invoke();
        }
    }

    [JSInvokable("ShowLatLonAsync")]
    public async Task ShowLatLonAsync(string latLon)
    {
        LatLon = latLon;
        
        if (NotifyLatLng != null)
        {
            await NotifyLatLng?.Invoke();
        }
    }
}