namespace Poseidon.Client.ApiServices.Interfaces;

public interface IVesselApiService
{
    Task<GetVesselsResponseDto> GetVesselsAsync();

    Task<GetVesselResponseDto> GetVesselAsync(string mmsi);

    Task<InsertVesselResponseDto> InsertVesselAsync(InsertVesselRequestDto insertVesselRequestDto);

    Task<UpdateVesselResponseDto> UpdateVesselAsync(int id, UpdateVesselRequestDto updateVesselRequestDto);

    Task<DeleteVesselResponseDto> DeleteVesselAsync(int id);
}