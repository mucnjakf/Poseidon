namespace Poseidon.Client.Pages.Account.Modals;

public partial class InsertUserModal
{
    [Inject] private IValidator<InsertUserRequestDto> RegisterRequestDtoValidator { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    private InsertUserRequestDto InsertUserRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleInsertUserAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            InsertUserResponseDto response = await AccountApiService.InsertUserAsync(InsertUserRequestDto);

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
        ValidationResult validationResult = RegisterRequestDtoValidator.Validate(InsertUserRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}