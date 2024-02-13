namespace Poseidon.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginRequestDto loginRequestDto)
    {
        LoginResponseDto loginResponseDto = await _accountService.LoginAsync(loginRequestDto);

        return loginResponseDto.Successful ? Ok(loginResponseDto) : BadRequest(loginResponseDto);
    }

    [HttpGet("users")]
    public ActionResult<GetUsersResponseDto> GetUsers()
    {
        GetUsersResponseDto getUsersResponseDto = _accountService.GetUsers();

        return Ok(getUsersResponseDto);
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUserAsync([FromRoute] string username)
    {
        GetUserResponseDto getUserResponseDto = await _accountService.GetUserAsync(username);

        return getUserResponseDto.Successful ? Ok(getUserResponseDto) : BadRequest(getUserResponseDto);
    }

    [HttpPost("user")]
    public async Task<ActionResult<InsertUserResponseDto>> InsertUserAsync([FromBody] InsertUserRequestDto insertUserRequestDto)
    {
        InsertUserResponseDto insertUserResponseDto = await _accountService.InsertUserAsync(insertUserRequestDto);

        return insertUserResponseDto.Successful ? Ok(insertUserResponseDto) : BadRequest(insertUserResponseDto);
    }

    [HttpPut("user/email")]
    public async Task<ActionResult<UpdateUserEmailResponseDto>> UpdateUserEmailAsync([FromBody] UpdateUserEmailRequestDto updateUserEmailRequestDto)
    {
        UpdateUserEmailResponseDto updateUserEmailResponseDto = await _accountService.UpdateUserEmailAsync(updateUserEmailRequestDto);

        return updateUserEmailResponseDto.Successful ? Ok(updateUserEmailResponseDto) : BadRequest(updateUserEmailResponseDto);
    }
    
    [HttpPut("user/username")]
    public async Task<ActionResult<UpdateUserUsernameResponseDto>> UpdateUserUsernameAsync([FromBody] UpdateUserUsernameRequestDto updateUserUsernameRequestDto)
    {
        UpdateUserUsernameResponseDto updateUserUsernameResponseDto = await _accountService.UpdateUserUsernameAsync(updateUserUsernameRequestDto);

        return updateUserUsernameResponseDto.Successful ? Ok(updateUserUsernameResponseDto) : BadRequest(updateUserUsernameResponseDto);
    }
    
    [HttpPut("user/password")]
    public async Task<ActionResult<UpdateUserPasswordResponseDto>> UpdateUserPasswordAsync([FromBody] UpdateUserPasswordRequestDto updateUserPasswordRequestDto)
    {
        UpdateUserPasswordResponseDto updateUserPasswordResponseDto = await _accountService.UpdateUserPasswordAsync(updateUserPasswordRequestDto);

        return updateUserPasswordResponseDto.Successful ? Ok(updateUserPasswordResponseDto) : BadRequest(updateUserPasswordResponseDto);
    }

    [HttpDelete("user/{username}")]
    public async Task<ActionResult<DeleteUserResponseDto>> DeleteUserAsync([FromRoute] string username)
    {
        DeleteUserResponseDto deleteUserResponseDto = await _accountService.DeleteUserAsync(username);

        return deleteUserResponseDto.Successful ? Ok(deleteUserResponseDto) : BadRequest(deleteUserResponseDto);
    }
}