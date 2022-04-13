namespace Poseidon.Client.Pages.Account.Modals;

public partial class UpdateUserEmailModal
{
    [Inject] private IValidator<UpdateUserEmailRequestDto> UpdateUserEmailRequestDtoValidator { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public string Username { get; set; }

    private UpdateUserEmailRequestDto UpdateUserEmailRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleUpdateUserEmailAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            UpdateUserEmailRequestDto.Username = Username;
            UpdateUserEmailResponseDto response = await AccountApiService.UpdateUserEmailAsync(UpdateUserEmailRequestDto);

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
        ValidationResult validationResult = UpdateUserEmailRequestDtoValidator.Validate(UpdateUserEmailRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}