namespace Poseidon.Shared.DTO.Account;

public class UpdateUserUsernameRequestDto
{
    public string CurrentUsername { get; set; }

    public string NewUsername { get; set; }
    
    public string ConfirmUsername { get; set; }
}