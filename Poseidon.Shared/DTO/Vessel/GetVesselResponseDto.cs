namespace Poseidon.Shared.DTO.Vessel;

public class GetVesselResponseDto
{
    public bool Successful { get; set; }

    public string Error { get; set; }

    public VesselModel VesselModel { get; set; }
}
