namespace Poseidon.Client.ApiServices;

public class EventApiService : IEventApiService
{
    private readonly HttpClient _httpClient;

    public EventApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<GetEventsResponseDto> GetEventsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/event");

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetEventsResponseDto response = JsonSerializer.Deserialize<GetEventsResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<GetEventsResponseDto> GetEventsAsync(string mmsi)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/event/vessel?mmsi={mmsi}");
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetEventsResponseDto response = JsonSerializer.Deserialize<GetEventsResponseDto>(stringResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<GetEventResponseDto> GetEventAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/event/{id}");

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetEventResponseDto response = JsonSerializer.Deserialize<GetEventResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<bool> DismissEventAsync(int id)
    {
        StringContent data = new(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PutAsync("api/event/dismiss", data);

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        bool response = JsonSerializer.Deserialize<bool>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task ReceiveEventsAsync(bool shouldReceiveEvents) => await _httpClient.GetAsync($"api/event/send/{shouldReceiveEvents}");
}