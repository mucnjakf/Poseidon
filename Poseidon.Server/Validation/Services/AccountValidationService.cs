namespace Poseidon.Server.Validation.Services;

public class AccountValidationService : IAccountValidationService
{
    private readonly IValidator<LoginRequestDto> _loginRequestDtoValidator;
    private readonly IValidator<InsertUserRequestDto> _insertUserRequestDtoValidator;
    private readonly IValidator<UpdateUserEmailRequestDto> _updateUserEmailRequestDtoValidator;
    private readonly IValidator<UpdateUserUsernameRequestDto> _updateUserUsernameRequestDtoValidator;
    private readonly IValidator<UpdateUserPasswordRequestDto> _updateUserPasswordRequestDtoValidator;

    public AccountValidationService(
        IValidator<LoginRequestDto> loginRequestDtoValidator, 
        IValidator<InsertUserRequestDto> insertUserRequestDtoValidator, 
        IValidator<UpdateUserEmailRequestDto> updateUserEmailRequestDtoValidator, 
        IValidator<UpdateUserUsernameRequestDto> updateUserUsernameRequestDtoValidator, 
        IValidator<UpdateUserPasswordRequestDto> updateUserPasswordRequestDtoValidator)
    {
        _loginRequestDtoValidator = loginRequestDtoValidator ?? throw new ArgumentNullException(nameof(loginRequestDtoValidator));
        _insertUserRequestDtoValidator = insertUserRequestDtoValidator ?? throw new ArgumentNullException(nameof(insertUserRequestDtoValidator));
        _updateUserEmailRequestDtoValidator = updateUserEmailRequestDtoValidator ?? throw new ArgumentNullException(nameof(updateUserEmailRequestDtoValidator));
        _updateUserUsernameRequestDtoValidator = updateUserUsernameRequestDtoValidator ?? throw new ArgumentNullException(nameof(updateUserUsernameRequestDtoValidator));
        _updateUserPasswordRequestDtoValidator = updateUserPasswordRequestDtoValidator ?? throw new ArgumentNullException(nameof(updateUserPasswordRequestDtoValidator));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateLoginRequestDto(LoginRequestDto loginRequestDto)
    {
        ValidationResult result = await _loginRequestDtoValidator.ValidateAsync(loginRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateInsertUserRequestDto(InsertUserRequestDto insertUserRequestDto)
    {
        ValidationResult result = await _insertUserRequestDtoValidator.ValidateAsync(insertUserRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserEmailRequestDto(UpdateUserEmailRequestDto updateUserEmailRequestDto)
    {
        ValidationResult result = await _updateUserEmailRequestDtoValidator.ValidateAsync(updateUserEmailRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserUsernameRequestDto(UpdateUserUsernameRequestDto updateUserUsernameRequestDto)
    {
        ValidationResult result = await _updateUserUsernameRequestDtoValidator.ValidateAsync(updateUserUsernameRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }

    public async Task<Tuple<bool, IEnumerable<string>>> ValidateUpdateUserPasswordRequestDto(UpdateUserPasswordRequestDto updateUserPasswordRequestDto)
    {
        ValidationResult result = await _updateUserPasswordRequestDtoValidator.ValidateAsync(updateUserPasswordRequestDto);

        return result.IsValid
            ? new Tuple<bool, IEnumerable<string>>(true, null)
            : new Tuple<bool, IEnumerable<string>>(false, result.Errors.Select(error => error.ErrorMessage));
    }
}