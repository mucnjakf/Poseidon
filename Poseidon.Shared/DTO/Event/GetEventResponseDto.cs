namespace Poseidon.Shared.DTO.Event;

public class GetEventResponseDto
{
    public bool Successful { get; set; }

    public string Error { get; set; }

    public EventModel EventModel { get; set; }
}