namespace Poseidon.Client.Validation.Account;

public class InsertUserRequestDtoValidator : AbstractValidator<InsertUserRequestDto>
{
    public InsertUserRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-Mail address is required.");

        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Password and Confirm Password do not match.");
    }
}