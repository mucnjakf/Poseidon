namespace Poseidon.Shared.DTO.Account;

public class UpdateUserPasswordRequestDto
{
    public string Username { get; set; }

    public string CurrentPassword { get; set; }
    
    public string NewPassword { get; set; }

    public string ConfirmPassword { get; set; }
}