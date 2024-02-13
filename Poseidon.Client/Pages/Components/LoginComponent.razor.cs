namespace Poseidon.Client.Pages.Components;

public partial class LoginComponent
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IValidator<LoginRequestDto> LoginRequestDtoValidator { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    private LoginRequestDto LoginRequestDto { get; } = new();

    private bool ShowSpinner { get; set; }

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; } = new();

    private async Task HandleLoginAsync()
    {
        ShowSpinner = true;
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            LoginResponseDto response = await AccountApiService.LoginAsync(LoginRequestDto);

            if (response.Successful)
            {
                NavigationManager.NavigateTo(NavigationManager.Uri);
                return;
            }

            Errors.AddRange(response.Errors);
            ShowErrors = true;
        }

        ShowSpinner = false;
    }

    private bool FormIsValid()
    {
        ValidationResult validationResult = LoginRequestDtoValidator.Validate(LoginRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}