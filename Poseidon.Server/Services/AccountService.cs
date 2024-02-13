namespace Poseidon.Server.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IAccountValidationService _accountValidationService;
    private readonly UserManager<ApplicationUserEntity> _userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager;

    public AccountService(
        IConfiguration configuration,
        IMapper mapper,
        IAccountValidationService accountValidationService,
        UserManager<ApplicationUserEntity> userManager,
        SignInManager<ApplicationUserEntity> signInManager)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _accountValidationService = accountValidationService ?? throw new ArgumentNullException(nameof(accountValidationService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _accountValidationService.ValidateLoginRequestDto(loginRequestDto);

        if (!successful)
        {
            return new LoginResponseDto { Successful = false, Errors = errors };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(loginRequestDto.Username);

        if (applicationUserEntity is null)
        {
            return new LoginResponseDto { Successful = false, Errors = new List<string> { $"Username '{loginRequestDto.Username}' not found." } };
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(applicationUserEntity, loginRequestDto.Password, false, false);

        if (!result.Succeeded)
        {
            return new LoginResponseDto { Successful = false, Errors = new List<string> { "Password is invalid." } };
        }

        List<Claim> claims = new() { new Claim(ClaimTypes.Name, applicationUserEntity.UserName) };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));

        JwtSecurityToken jwtToken = new(
            _configuration["Jwt:ValidIssuer"],
            _configuration["Jwt:ValidAudience"],
            claims,
            expires: expiry,
            signingCredentials: credentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new LoginResponseDto { Successful = true, Token = token };
    }

    public GetUsersResponseDto GetUsers()
    {
        IEnumerable<ApplicationUserModel> applicationUsers = _userManager.Users.Select(entity => _mapper.Map<ApplicationUserModel>(entity));

        return new GetUsersResponseDto { Result = applicationUsers };
    }

    public async Task<GetUserResponseDto> GetUserAsync(string username)
    {
        if (username is null)
        {
            return new GetUserResponseDto { Successful = false, Error = "Username must be set." };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(username);

        if (applicationUserEntity is null)
        {
            return new GetUserResponseDto { Successful = false, Error = $"Username {username} not found." };
        }

        ApplicationUserModel applicationUserModel = _mapper.Map<ApplicationUserModel>(applicationUserEntity);

        return new GetUserResponseDto { Successful = true, ApplicationUserModel = applicationUserModel };
    }

    public async Task<InsertUserResponseDto> InsertUserAsync(InsertUserRequestDto insertUserRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _accountValidationService.ValidateInsertUserRequestDto(insertUserRequestDto);

        if (!successful)
        {
            return new InsertUserResponseDto { Successful = false, Errors = errors };
        }

        ApplicationUserEntity applicationUserEntity = new() { UserName = insertUserRequestDto.Username, Email = insertUserRequestDto.Email };

        IdentityResult result = await _userManager.CreateAsync(applicationUserEntity, insertUserRequestDto.Password);

        return result.Succeeded
            ? new InsertUserResponseDto { Successful = true }
            : new InsertUserResponseDto { Successful = false, Errors = result.Errors.Select(error => error.Description) };
    }

    public async Task<UpdateUserEmailResponseDto> UpdateUserEmailAsync(UpdateUserEmailRequestDto updateUserEmailRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _accountValidationService.ValidateUpdateUserEmailRequestDto(updateUserEmailRequestDto);

        if (!successful)
        {
            return new UpdateUserEmailResponseDto { Successful = false, Errors = errors };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(updateUserEmailRequestDto.Username);

        if (applicationUserEntity is null)
        {
            return new UpdateUserEmailResponseDto
                { Successful = false, Errors = new List<string> { $"User with username '{updateUserEmailRequestDto.Username}' not found." } };
        }

        string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(applicationUserEntity, updateUserEmailRequestDto.Email);

        IdentityResult result = await _userManager.ChangeEmailAsync(applicationUserEntity, updateUserEmailRequestDto.Email, changeEmailToken);

        return result.Succeeded
            ? new UpdateUserEmailResponseDto { Successful = true }
            : new UpdateUserEmailResponseDto { Successful = false, Errors = result.Errors.Select(error => error.Description) };
    }

    public async Task<UpdateUserUsernameResponseDto> UpdateUserUsernameAsync(UpdateUserUsernameRequestDto updateUserUsernameRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _accountValidationService.ValidateUpdateUserUsernameRequestDto(updateUserUsernameRequestDto);

        if (!successful)
        {
            return new UpdateUserUsernameResponseDto { Successful = false, Errors = errors };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(updateUserUsernameRequestDto.CurrentUsername);

        if (applicationUserEntity is null)
        {
            return new UpdateUserUsernameResponseDto
            {
                Successful = false, Errors = new List<string> { $"User with username '{updateUserUsernameRequestDto.CurrentUsername}' not found." }
            };
        }

        IdentityResult result = await _userManager.SetUserNameAsync(applicationUserEntity, updateUserUsernameRequestDto.NewUsername);

        return result.Succeeded
            ? new UpdateUserUsernameResponseDto { Successful = true }
            : new UpdateUserUsernameResponseDto { Successful = false, Errors = result.Errors.Select(error => error.Description) };
    }

    public async Task<UpdateUserPasswordResponseDto> UpdateUserPasswordAsync(UpdateUserPasswordRequestDto updateUserPasswordRequestDto)
    {
        (bool successful, IEnumerable<string> errors) = await _accountValidationService.ValidateUpdateUserPasswordRequestDto(updateUserPasswordRequestDto);

        if (!successful)
        {
            return new UpdateUserPasswordResponseDto { Successful = false, Errors = errors };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(updateUserPasswordRequestDto.Username);

        if (applicationUserEntity is null)
        {
            return new UpdateUserPasswordResponseDto
                { Successful = false, Errors = new List<string> { $"User with username '{updateUserPasswordRequestDto.Username}' not found." } };
        }

        IdentityResult result = await _userManager.ChangePasswordAsync(applicationUserEntity,
            updateUserPasswordRequestDto.CurrentPassword,
            updateUserPasswordRequestDto.NewPassword);

        return result.Succeeded
            ? new UpdateUserPasswordResponseDto { Successful = true }
            : new UpdateUserPasswordResponseDto { Successful = false, Errors = result.Errors.Select(error => error.Description) };
    }

    public async Task<DeleteUserResponseDto> DeleteUserAsync(string username)
    {
        if (username is null)
        {
            return new DeleteUserResponseDto { Successful = false, Errors = new List<string> { "Username must me set." } };
        }

        ApplicationUserEntity applicationUserEntity = await _userManager.FindByNameAsync(username);

        if (applicationUserEntity is null)
        {
            return new DeleteUserResponseDto { Successful = false, Errors = new List<string> { $"User with username '{username}' not found." } };
        }

        IdentityResult result = await _userManager.DeleteAsync(applicationUserEntity);

        return result.Succeeded
            ? new DeleteUserResponseDto { Successful = true }
            : new DeleteUserResponseDto { Successful = false, Errors = result.Errors.Select(error => error.Description) };
    }
}