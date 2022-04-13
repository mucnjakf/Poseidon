namespace Poseidon.Shared.DTO.Account;

public class LoginResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public string Token { get; set; }
}
