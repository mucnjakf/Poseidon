namespace Poseidon.Client.Pages.Account.Modals;

public partial class UpdateUserPasswordModal
{
    [Inject] private IValidator<UpdateUserPasswordRequestDto> UpdateUserPasswordRequestDtoValidator { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public string Username { get; set; }

    private UpdateUserPasswordRequestDto UpdateUserPasswordRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleUpdateUserPasswordAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            UpdateUserPasswordRequestDto.Username = Username;
            UpdateUserPasswordResponseDto response = await AccountApiService.UpdateUserPasswordAsync(UpdateUserPasswordRequestDto);

            if (response.Successful)
            {
                await ModalInstance.CloseAsync();
                return;
            }

            Errors = response.Errors.ToList();
            ShowErrors = true;
        }
    }

    private bool FormIsValid()
    {
        ValidationResult validationResult = UpdateUserPasswordRequestDtoValidator.Validate(UpdateUserPasswordRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}