namespace Poseidon.Shared.DTO.Account;

public class UpdateUserEmailResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}