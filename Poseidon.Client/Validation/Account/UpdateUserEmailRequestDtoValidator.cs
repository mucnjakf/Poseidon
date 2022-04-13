namespace Poseidon.Client.Validation.Account;

public class UpdateUserEmailRequestDtoValidator : AbstractValidator<UpdateUserEmailRequestDto>
{
    public UpdateUserEmailRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-Mail address is required.");

        RuleFor(x => x.Email).Equal(x => x.ConfirmEmail).WithMessage("E-Mail and Confirm E-Mail do not match.");
    }
}