namespace Poseidon.Server.Validation.Account;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username).NotNull().WithMessage("Username must be set.");
        
        RuleFor(x => x.Password).NotNull().WithMessage("Password must be set.");
    }
}