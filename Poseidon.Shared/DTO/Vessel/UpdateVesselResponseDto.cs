namespace Poseidon.Shared.DTO.Vessel;

public class UpdateVesselResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}