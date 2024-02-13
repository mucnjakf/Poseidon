namespace Poseidon.Server.Services.Interfaces;

public interface IEventService
{
    GetEventsResponseDto GetEvents();

    GetEventsResponseDto GetEvents(string mmsi);
    
    Task<GetEventResponseDto> GetEventAsync(int id);

    Task<InsertEventResponseDto> InsertEventAsync(InsertEventRequestDto insertEventRequestDto);

    Task<bool> DismissEventAsync(int id);

    Task StartSendingEventsAsync();
}