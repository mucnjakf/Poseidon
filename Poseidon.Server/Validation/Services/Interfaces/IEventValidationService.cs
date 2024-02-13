namespace Poseidon.Server.Validation.Services.Interfaces;

public interface IEventValidationService
{
    Task<Tuple<bool, IEnumerable<string>>> ValidateInsertEventRequestDto(InsertEventRequestDto insertEventRequestDto);
}