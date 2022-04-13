namespace Poseidon.Client.Pages.Account.Modals;

public partial class DeleteUserModal
{
    [Inject] private IAccountApiService AccountApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public string Username { get; set; }

    [Parameter] public string Message { get; set; }

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleDeleteUserAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        DeleteUserResponseDto response = await AccountApiService.DeleteUserAsync(Username);

        if (response.Successful)
        {
            await ModalInstance.CloseAsync();
            return;
        }

        Errors = response.Errors.ToList();
        ShowErrors = true;
    }
}