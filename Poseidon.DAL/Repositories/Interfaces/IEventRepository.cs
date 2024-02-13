namespace Poseidon.DAL.Repositories.Interfaces;

public interface IEventRepository
{
    IEnumerable<EventEntity> GetEvents();

    IEnumerable<EventEntity> GetEvents(string mmsi);

    Task<EventEntity> GetEventAsync(int id);

    Task<bool> InsertEventAsync(EventEntity eventEntity);

    Task<bool> DismissEventAsync(int id);

    Task<LatestEventEntity> GetLatestUnprocessedEventAsync();
}