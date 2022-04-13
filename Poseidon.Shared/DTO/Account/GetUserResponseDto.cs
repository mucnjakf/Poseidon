namespace Poseidon.Shared.DTO.Account;

public class GetUserResponseDto
{
    public bool Successful { get; set; }
    
    public string Error { get; set; }
    
    public ApplicationUserModel ApplicationUserModel { get; set; }
}