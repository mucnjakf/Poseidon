namespace Poseidon.Shared.DTO.Account;

public class DeleteUserResponseDto
{
    public bool Successful { get; set; }

    public IEnumerable<string> Errors { get; set; }
}
