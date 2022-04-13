namespace Poseidon.Client.Pages.Account.Modals;

public partial class UpdateUserUsernameModal
{
    [Inject] private IValidator<UpdateUserUsernameRequestDto> UpdateUserUsernameRequestDtoValidator { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public string Username { get; set; }

    private UpdateUserUsernameRequestDto UpdateUserUsernameRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleUpdateUserUsernameAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            UpdateUserUsernameRequestDto.CurrentUsername = Username;
            UpdateUserUsernameResponseDto response = await AccountApiService.UpdateUserUsernameAsync(UpdateUserUsernameRequestDto);

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
        ValidationResult validationResult = UpdateUserUsernameRequestDtoValidator.Validate(UpdateUserUsernameRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}