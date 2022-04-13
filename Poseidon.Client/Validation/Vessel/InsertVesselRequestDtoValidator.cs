namespace Poseidon.Client.Validation.Vessel;

public class InsertVesselRequestDtoValidator : AbstractValidator<InsertVesselRequestDto>
{
    public InsertVesselRequestDtoValidator()
    {
        RuleFor(x => x.Mmsi).NotEmpty().WithMessage("MMSI must be set.");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be set.");

        RuleFor(x => x.Callsign).NotEmpty().WithMessage("Callsign must be set.");

        RuleFor(x => x.Flag).NotEmpty().WithMessage("Flag must be set.");

        RuleFor(x => x.Imo).NotEmpty().WithMessage("IMO must be set.");
    }
}