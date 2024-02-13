namespace Poseidon.Client.ApiServices.Interfaces;

public interface IAccountApiService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);

    Task LogoutAsync();

    Task<GetUsersResponseDto> GetUsersAsync();

    Task<GetUserResponseDto> GetUserAsync(string username);

    Task<InsertUserResponseDto> InsertUserAsync(InsertUserRequestDto insertUserRequestDto);

    Task<UpdateUserEmailResponseDto> UpdateUserEmailAsync(UpdateUserEmailRequestDto updateUserEmailRequestDto);

    Task<UpdateUserUsernameResponseDto> UpdateUserUsernameAsync(UpdateUserUsernameRequestDto updateUserUsernameRequestDto);

    Task<UpdateUserPasswordResponseDto> UpdateUserPasswordAsync(UpdateUserPasswordRequestDto updateUserPasswordRequestDto);

    Task<DeleteUserResponseDto> DeleteUserAsync(string username);
}