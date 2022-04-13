namespace Poseidon.Shared.DTO.Event;

public class InsertEventResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}