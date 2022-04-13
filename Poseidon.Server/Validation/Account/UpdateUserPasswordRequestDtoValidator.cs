namespace Poseidon.Server.Validation.Account;

public class UpdateUserPasswordRequestDtoValidator : AbstractValidator<UpdateUserPasswordRequestDto>
{
    public UpdateUserPasswordRequestDtoValidator()
    {
        RuleFor(x => x.Username).NotNull().WithMessage("Username must be set.");
        
        RuleFor(x => x.CurrentPassword).NotNull().WithMessage("Current Password must be set.");
        
        RuleFor(x => x.NewPassword).NotNull().WithMessage("New Password must be set.");
    }
}