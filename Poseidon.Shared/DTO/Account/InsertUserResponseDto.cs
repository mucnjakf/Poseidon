namespace Poseidon.Shared.DTO.Account;

public class InsertUserResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}