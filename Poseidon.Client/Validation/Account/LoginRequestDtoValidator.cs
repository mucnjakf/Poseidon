namespace Poseidon.Client.Validation.Account;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}