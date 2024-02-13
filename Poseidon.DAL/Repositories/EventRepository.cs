namespace Poseidon.DAL.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    private static readonly Random Rng = new();

    public EventRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<EventEntity> GetEvents() => _context.Events;

    public IEnumerable<EventEntity> GetEvents(string mmsi) => _context.Events.Where(@event => @event.Mmsi == mmsi);

    public async Task<EventEntity> GetEventAsync(int id)
    {
        EventEntity eventEntity = await _context.Events.FirstOrDefaultAsync(@event => @event.Id == id);

        return eventEntity;
    }

    public async Task<bool> InsertEventAsync(EventEntity eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);

        int numberOfEntriesWritten = await _context.SaveChangesAsync();

        return numberOfEntriesWritten > 0;
    }

    public async Task<bool> DismissEventAsync(int id)
    {
        EventEntity eventEntity = await GetEventAsync(id);

        eventEntity.IsDismissed = true;
        int numberOfEntriesWritten = await _context.SaveChangesAsync();

        return numberOfEntriesWritten > 0;
    }

    public async Task<LatestEventEntity> GetLatestUnprocessedEventAsync()
    {
        LatestEventEntity latestEventEntity = _context.LatestEvents
            .ToList()
            .OrderBy(_ => Rng.Next())
            .FirstOrDefault(latestEvent => latestEvent.IsProcessed == false);

        // ReSharper disable once InlineTemporaryVariable
        LatestEventEntity latestEventEntityToReturn = latestEventEntity;

        latestEventEntity!.IsProcessed = true;
        await _context.SaveChangesAsync();

        return latestEventEntityToReturn;
    }
}