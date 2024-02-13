namespace Poseidon.Shared.DTO.Event;

public class GetEventsResponseDto
{
    public IEnumerable<EventModel> Result { get; set; }

    public bool Successful { get; set; }

    public string Error { get; set; }
}