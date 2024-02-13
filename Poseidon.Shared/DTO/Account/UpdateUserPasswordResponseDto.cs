namespace Poseidon.Shared.DTO.Account;

public class UpdateUserPasswordResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}