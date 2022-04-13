namespace Poseidon.Client.Validation.Account;

public class UpdateUserPasswordRequestDtoValidator : AbstractValidator<UpdateUserPasswordRequestDto>
{
    public UpdateUserPasswordRequestDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current Password is required.");

        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New Password is required.");

        RuleFor(x => x.NewPassword).Equal(x => x.ConfirmPassword).WithMessage("New Password and Confirm Password do not match.");
    }
}