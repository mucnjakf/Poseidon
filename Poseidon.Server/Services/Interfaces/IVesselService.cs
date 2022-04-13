namespace Poseidon.Server.Services.Interfaces;

public interface IVesselService
{
    GetVesselsResponseDto GetVessels();

    Task<GetVesselResponseDto> GetVesselAsync(int id);

    Task<GetVesselResponseDto> GetVesselAsync(string mmsi);

    Task<InsertVesselResponseDto> InsertVesselAsync(InsertVesselRequestDto insertVesselRequestDto);

    Task<UpdateVesselResponseDto> UpdateVesselAsync(int id, UpdateVesselRequestDto updateVesselRequestDto);

    Task<DeleteVesselResponseDto> DeleteVesselAsync(int id);
}