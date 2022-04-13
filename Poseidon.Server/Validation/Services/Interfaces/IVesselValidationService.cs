namespace Poseidon.Server.Validation.Services.Interfaces;

public interface IVesselValidationService
{
    Task<Tuple<bool, IEnumerable<string>>> ValidateInsertVesselRequestDto(InsertVesselRequestDto insertVesselRequestDto);
    
    Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateVesselRequestDto(UpdateVesselRequestDto updateVesselRequestDto);
}