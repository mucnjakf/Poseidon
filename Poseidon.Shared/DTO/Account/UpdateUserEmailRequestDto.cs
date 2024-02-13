namespace Poseidon.Shared.DTO.Account;

public class UpdateUserEmailRequestDto
{
    public string Username { get; set; }

    public string Email { get; set; }
    
    public string ConfirmEmail { get; set; }
}