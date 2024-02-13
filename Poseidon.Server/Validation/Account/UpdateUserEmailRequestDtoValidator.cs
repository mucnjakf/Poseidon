namespace Poseidon.Server.Validation.Account;

public class UpdateUserEmailRequestDtoValidator : AbstractValidator<UpdateUserEmailRequestDto>
{
    public UpdateUserEmailRequestDtoValidator()
    {
        RuleFor(x => x.Username).NotNull().WithMessage("Username must be set.");

        RuleFor(x => x.Email).NotNull().WithMessage("E-Mail must be set.");
    }
}