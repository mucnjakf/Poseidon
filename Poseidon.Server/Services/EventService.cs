namespace Poseidon.Server.Services;

public class EventService : IEventService
{
    private readonly IMapper _mapper;
    private readonly EventHub _eventHub;
    private readonly IEventValidationService _eventValidationService;
    private readonly IEventRepository _eventRepository;

    public EventService(
        IMapper mapper,
        EventHub eventHub,
        IEventValidationService eventValidationService,
        IEventRepository eventRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventHub = eventHub ?? throw new ArgumentNullException(nameof(eventHub));
        _eventValidationService = eventValidationService ?? throw new ArgumentNullException(nameof(eventValidationService));
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    public GetEventsResponseDto GetEvents()
    {
        IEnumerable<EventModel> events = _eventRepository.GetEvents().Select(@event => _mapper.Map<EventModel>(@event));

        return new GetEventsResponseDto { Successful = true, Result = events };
    }

    public GetEventsResponseDto GetEvents(string mmsi)
    {
        if (string.IsNullOrWhiteSpace(mmsi))
        {
            return new GetEventsResponseDto { Successful = false, Error = "Vessel MMSI is not valid." };
        }

        IEnumerable<EventModel> events = _eventRepository.GetEvents(mmsi).Select(@event => _mapper.Map<EventModel>(@event));

        return new GetEventsResponseDto { Successful = true, Result = events };
    }

    public async Task<GetEventResponseDto> GetEventAsync(int id)
    {
        if (id <= 0)
        {
            return new GetEventResponseDto { Successful = false, Error = "ID is not valid." };
        }

        EventEntity eventEntity = await _eventRepository.GetEventAsync(id);

        if (eventEntity is null)
        {
            return new GetEventResponseDto { Successful = false, Error = $"Vessel with ID {id} was not found." };
        }

        EventModel eventModel = _mapper.Map<EventModel>(eventEntity);

        return new GetEventResponseDto { Successful = true, EventModel = eventModel };
    }

    public async Task<InsertEventResponseDto> InsertEventAsync(InsertEventRequestDto insertEventRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _eventValidationService.ValidateInsertEventRequestDto(insertEventRequestDto);

        if (!successful)
        {
            return new InsertEventResponseDto { Successful = false, Errors = errors };
        }

        EventEntity eventEntity = _mapper.Map<EventEntity>(insertEventRequestDto);

        bool succeeded = await _eventRepository.InsertEventAsync(eventEntity);

        return succeeded
            ? new InsertEventResponseDto { Successful = true }
            : new InsertEventResponseDto { Successful = false };
    }
    
    public async Task<bool> DismissEventAsync(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        bool success = await _eventRepository.DismissEventAsync(id);
        return success;
    }

    public async Task StartSendingEventsAsync()
    {
        LatestEventEntity latestEventEntity = await _eventRepository.GetLatestUnprocessedEventAsync();

        EventEntity eventEntity = PredictIllegalEvent(latestEventEntity);

        await _eventRepository.InsertEventAsync(eventEntity);

        EventModel eventModel = _mapper.Map<EventModel>(eventEntity);

        await _eventHub.SendEventAsync(eventModel);
    }

    [SuppressMessage("ReSharper", "SpecifyACultureInStringConversionExplicitly")]
    private static EventEntity PredictIllegalEvent(LatestEventEntity latestEventEntity)
    {
        IllegalEventsModel.ModelInput mlEventData = new()
        {
            Mmsi = float.Parse(latestEventEntity.Mmsi),
            Latitude = float.Parse(latestEventEntity.Latitude),
            Longitude = float.Parse(latestEventEntity.Longitude),
            Start_timestamp = latestEventEntity.StartTimestamp.ToString(),
            End_timestamp = latestEventEntity.EndTimestamp.ToString(),
            Median_speed_knots = (float)latestEventEntity.MedianSpeedKnots,
            Total_event_duration_hours = (float)latestEventEntity.TotalDurationHours
        };

        IllegalEventsModel.ModelOutput result = IllegalEventsModel.Predict(mlEventData);

        return new EventEntity
        {
            Mmsi = latestEventEntity.Mmsi,
            Latitude = latestEventEntity.Latitude,
            Longitude = latestEventEntity.Longitude,
            StartTimestamp = latestEventEntity.StartTimestamp,
            EndTimestamp = latestEventEntity.EndTimestamp,
            MedianSpeedKnots = latestEventEntity.MedianSpeedKnots,
            TotalDurationHours = latestEventEntity.TotalDurationHours,
            IsIllegal = result.PredictedLabel,
            IllegalEventProbability = result.Probability * 100,
        };
    }
}