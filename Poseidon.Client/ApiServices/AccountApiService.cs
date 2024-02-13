namespace Poseidon.Client.ApiServices;

public class AccountApiService : IAccountApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AccountApiService(
        HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(loginRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/account/login", data);

        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        LoginResponseDto response = JsonSerializer.Deserialize<LoginResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (!httpResponse.IsSuccessStatusCode)
        {
            return response;
        }

        await _localStorage.SetItemAsync("token", response!.Token);

        ((ApiAuthenticationStateProvider) _authenticationStateProvider).MarkUserAsAuthenticated(loginRequestDto.Username);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

        return response;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("token");

        ((ApiAuthenticationStateProvider) _authenticationStateProvider).MarkUserAsLoggedOut();

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<GetUsersResponseDto> GetUsersAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/account/users");
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetUsersResponseDto response = JsonSerializer.Deserialize<GetUsersResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        return response;
    }
    
    public async Task<GetUserResponseDto> GetUserAsync(string username)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/account/user/{username}");
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        GetUserResponseDto response = JsonSerializer.Deserialize<GetUserResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<InsertUserResponseDto> InsertUserAsync(InsertUserRequestDto insertUserRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(insertUserRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("api/account/user", data);
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        InsertUserResponseDto response = JsonSerializer.Deserialize<InsertUserResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }
    
    public async Task<UpdateUserEmailResponseDto> UpdateUserEmailAsync(UpdateUserEmailRequestDto updateUserEmailRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(updateUserEmailRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PutAsync("api/account/user/email", data);
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        UpdateUserEmailResponseDto response = JsonSerializer.Deserialize<UpdateUserEmailResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }
    
    public async Task<UpdateUserUsernameResponseDto> UpdateUserUsernameAsync(UpdateUserUsernameRequestDto updateUserUsernameRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(updateUserUsernameRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PutAsync("api/account/user/username", data);
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        UpdateUserUsernameResponseDto response = JsonSerializer.Deserialize<UpdateUserUsernameResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }
    
    public async Task<UpdateUserPasswordResponseDto> UpdateUserPasswordAsync(UpdateUserPasswordRequestDto updateUserPasswordRequestDto)
    {
        StringContent data = new(JsonSerializer.Serialize(updateUserPasswordRequestDto), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await _httpClient.PutAsync("api/account/user/password", data);
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        UpdateUserPasswordResponseDto response = JsonSerializer.Deserialize<UpdateUserPasswordResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return response;
    }

    public async Task<DeleteUserResponseDto> DeleteUserAsync(string username)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"api/account/user/{username}");
        
        string stringResponse = await httpResponse.Content.ReadAsStringAsync();
        DeleteUserResponseDto userResponse = JsonSerializer.Deserialize<DeleteUserResponseDto>(stringResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return userResponse;
    }
}