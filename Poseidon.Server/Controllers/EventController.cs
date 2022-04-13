namespace Poseidon.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/event")]
public class EventController : ControllerBase
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IEventService _eventService;

    public EventController(IRecurringJobManager recurringJobManager, IEventService eventService)
    {
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
    }

    [HttpGet]
    public ActionResult<GetEventsResponseDto> GetEvents()
    {
        GetEventsResponseDto getEventsResponseDto = _eventService.GetEvents();

        return Ok(getEventsResponseDto);
    }

    [HttpGet("vessel")]
    public ActionResult<GetEventsResponseDto> GetEvents([FromQuery] string mmsi)
    {
        GetEventsResponseDto getEventsResponseDto = _eventService.GetEvents(mmsi);

        return getEventsResponseDto.Successful ? Ok(getEventsResponseDto) : BadRequest(getEventsResponseDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetEventResponseDto>> GetEventAsync([FromRoute] int id)
    {
        GetEventResponseDto getEventResponseDto = await _eventService.GetEventAsync(id);

        return getEventResponseDto.Successful ? Ok(getEventResponseDto) : BadRequest(getEventResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult<InsertEventRequestDto>> InsertEventAsync([FromBody] InsertEventRequestDto insertEventRequestDto)
    {
        InsertEventResponseDto insertEventResponseDto = await _eventService.InsertEventAsync(insertEventRequestDto);

        return insertEventResponseDto.Successful ? Ok(insertEventResponseDto) : BadRequest(insertEventResponseDto);
    }

    [HttpPut("dismiss")]
    public async Task<ActionResult<bool>> DismissEventAsync([FromBody] int id)
    {
        bool success = await _eventService.DismissEventAsync(id);

        return success ? Ok(true) : BadRequest(false);
    }

    [HttpGet("send/{shouldSendEvents:bool}")]
    public ActionResult SendEvents([FromRoute] bool shouldSendEvents)
    {
        if (shouldSendEvents)
        {
            _recurringJobManager.AddOrUpdate("SendEvents", () => _eventService.StartSendingEventsAsync(), "* * * * *");
        }
        else
        {
            _recurringJobManager.RemoveIfExists("SendEvents");
        }

        return Ok();
    }
}