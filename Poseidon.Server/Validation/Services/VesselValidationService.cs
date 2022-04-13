namespace Poseidon.Server.Validation.Services;

public class VesselValidationService : IVesselValidationService
{
    private readonly IValidator<InsertVesselRequestDto> _insertVesselRequestDtoValidator;
    private readonly IValidator<UpdateVesselRequestDto> _updateVesselRequestDtoValidator;

    public VesselValidationService(
        IValidator<InsertVesselRequestDto> insertVesselRequestDtoValidator, 
        IValidator<UpdateVesselRequestDto> updateVesselRequestDtoValidator)
    {
        _insertVesselRequestDtoValidator =
            insertVesselRequestDtoValidator ?? throw new ArgumentNullException(nameof(insertVesselRequestDtoValidator));
        _updateVesselRequestDtoValidator = updateVesselRequestDtoValidator ?? throw new ArgumentNullException(nameof(updateVesselRequestDtoValidator));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateInsertVesselRequestDto(InsertVesselRequestDto insertVesselRequestDto)
    {
        ValidationResult result = await _insertVesselRequestDtoValidator.ValidateAsync(insertVesselRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateVesselRequestDto(UpdateVesselRequestDto updateVesselRequestDto)
    {
        ValidationResult result = await _updateVesselRequestDtoValidator.ValidateAsync(updateVesselRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }
}