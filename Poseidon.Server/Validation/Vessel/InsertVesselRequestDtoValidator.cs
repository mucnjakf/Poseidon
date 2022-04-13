namespace Poseidon.Server.Validation.Vessel;

public class InsertVesselRequestDtoValidator : AbstractValidator<InsertVesselRequestDto>
{
    public InsertVesselRequestDtoValidator()
    {
        RuleFor(x => x.Mmsi).NotNull().WithMessage("MMSI must be set.");
        
        RuleFor(x => x.Name).NotNull().WithMessage("Name must be set.");
        
        RuleFor(x => x.Callsign).NotNull().WithMessage("Callsign must be set.");
        
        RuleFor(x => x.Flag).NotNull().WithMessage("Flag must be set.");
        
        RuleFor(x => x.Imo).NotNull().WithMessage("IMO must be set.");
    }
}