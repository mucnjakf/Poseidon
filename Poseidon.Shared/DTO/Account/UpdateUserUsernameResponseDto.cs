namespace Poseidon.Shared.DTO.Account;

public class UpdateUserUsernameResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}