namespace Poseidon.Shared.DTO.Vessel;

public class InsertVesselRequestDto
{
    public string Mmsi { get; set; }

    public string Name { get; set; }

    public string Callsign { get; set; }

    public string Flag { get; set; }

    public string Imo { get; set; }
}