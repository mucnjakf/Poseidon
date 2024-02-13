namespace Poseidon.Server.Validation.Account;

public class InsertUserRequestDtoValidator : AbstractValidator<InsertUserRequestDto>
{
    public InsertUserRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotNull().WithMessage("E-Mail must be set.");
        
        RuleFor(x => x.Username).NotNull().WithMessage("Username must be set.");
        
        RuleFor(x => x.Password).NotNull().WithMessage("Password must be set.");
    }   
}