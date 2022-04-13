namespace Poseidon.Server.Validation.Services;

public class EventValidationService : IEventValidationService
{
    private readonly IValidator<InsertEventRequestDto> _insertEventRequestDtoValidator;

    public EventValidationService(IValidator<InsertEventRequestDto> insertEventRequestDtoValidator)
    {
        _insertEventRequestDtoValidator = insertEventRequestDtoValidator ?? throw new ArgumentNullException(nameof(insertEventRequestDtoValidator));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateInsertEventRequestDto(InsertEventRequestDto insertEventRequestDto)
    {
        ValidationResult result = await _insertEventRequestDtoValidator.ValidateAsync(insertEventRequestDto);
        
        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }
}