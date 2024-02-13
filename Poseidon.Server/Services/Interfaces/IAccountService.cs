namespace Poseidon.Server.Services.Interfaces;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);

    GetUsersResponseDto GetUsers();

    Task<GetUserResponseDto> GetUserAsync(string username);

    Task<InsertUserResponseDto> InsertUserAsync(InsertUserRequestDto insertUserRequestDto);

    Task<UpdateUserEmailResponseDto> UpdateUserEmailAsync(UpdateUserEmailRequestDto updateUserEmailRequestDto);

    Task<UpdateUserUsernameResponseDto> UpdateUserUsernameAsync(UpdateUserUsernameRequestDto updateUserUsernameRequestDto);

    Task<UpdateUserPasswordResponseDto> UpdateUserPasswordAsync(UpdateUserPasswordRequestDto updateUserPasswordRequestDto);

    Task<DeleteUserResponseDto> DeleteUserAsync(string username);
}