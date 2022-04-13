namespace Poseidon.Shared.DTO.Vessel;

public class InsertVesselResponseDto
{
    public bool Successful { get; set; }
    
    public IEnumerable<string> Errors { get; set; }
}
