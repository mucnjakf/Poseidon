namespace Poseidon.Server.Validation.Services.Interfaces;

public interface IAccountValidationService
{
    Task<Tuple<bool, IEnumerable<string>>> ValidateLoginRequestDto(LoginRequestDto loginRequestDto);
    
    Task<Tuple<bool, IEnumerable<string>>> ValidateInsertUserRequestDto(InsertUserRequestDto insertUserRequestDto);
    
    Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserEmailRequestDto(UpdateUserEmailRequestDto updateUserEmailRequestDto);
    
    Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserUsernameRequestDto(UpdateUserUsernameRequestDto updateUserUsernameRequestDto);
    
    Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserPasswordRequestDto(UpdateUserPasswordRequestDto updateUserPasswordRequestDto);

}