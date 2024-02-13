namespace Poseidon.Shared.DTO.Account;

public class InsertUserRequestDto
{
    public string Email { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}
