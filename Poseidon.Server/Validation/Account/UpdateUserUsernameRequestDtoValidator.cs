namespace Poseidon.Server.Validation.Account;

public class UpdateUserUsernameRequestDtoValidator : AbstractValidator<UpdateUserUsernameRequestDto>
{
    public UpdateUserUsernameRequestDtoValidator()
    {
        RuleFor(x => x.CurrentUsername).NotNull().WithMessage("Current Username must be set.");
        
        RuleFor(x => x.NewUsername).NotNull().WithMessage("New Username must be set.");
    }
}