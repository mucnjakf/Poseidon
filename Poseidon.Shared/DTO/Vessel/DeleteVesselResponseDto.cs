namespace Poseidon.Shared.DTO.Vessel;

public class DeleteVesselResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}