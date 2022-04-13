namespace Poseidon.Client.ApiServices.Interfaces;

public interface IEventApiService
{
    Task<GetEventsResponseDto> GetEventsAsync();

    Task<GetEventsResponseDto> GetEventsAsync(string mmsi);

    Task<GetEventResponseDto> GetEventAsync(int id);

    Task<bool> DismissEventAsync(int id);

    Task ReceiveEventsAsync(bool shouldReceiveEvents);
}