namespace Poseidon.Client.Validation.Account;

public class UpdateUserUsernameRequestDtoValidator : AbstractValidator<UpdateUserUsernameRequestDto>
{
    public UpdateUserUsernameRequestDtoValidator()
    {
        RuleFor(x => x.NewUsername).NotEmpty().WithMessage("Username is required.");
        
        RuleFor(x => x.NewUsername).Equal(x => x.ConfirmUsername).WithMessage("Username and Confirm Username do not match.");
    }
}