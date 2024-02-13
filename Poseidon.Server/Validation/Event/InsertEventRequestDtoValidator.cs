namespace Poseidon.Server.Validation.Event;

public class InsertEventRequestDtoValidator : AbstractValidator<InsertEventRequestDto>
{
    public InsertEventRequestDtoValidator()
    {
        RuleFor(x => x.Mmsi).NotNull().WithMessage("MMSI must be set.");
        
        RuleFor(x => x.Latitude).NotNull().WithMessage("Latitude must be set.");
        
        RuleFor(x => x.Longitude).NotNull().WithMessage("Callsign must be set.");
        
        RuleFor(x => x.StartTimestamp).NotNull().WithMessage("Start timestamp must be set.");
        
        RuleFor(x => x.EndTimestamp).NotNull().WithMessage("End timestamp must be set.");
        
        RuleFor(x => x.MedianSpeedKnots).NotNull().WithMessage("Median speed in knots must be set.");
        
        RuleFor(x => x.TotalDurationHours).NotNull().WithMessage("Total duration in hours must be set.");
        
        RuleFor(x => x.IsIllegal).NotNull().WithMessage("Is illegal flag must be set.");
    }
}