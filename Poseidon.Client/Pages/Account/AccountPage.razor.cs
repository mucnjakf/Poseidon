namespace Poseidon.Client.Pages.Account;

public partial class AccountPage
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [Inject] private IToastService ToastService { get; set; }
    
    [CascadingParameter] private IModalService ModalService { get; set; }

    private ApplicationUserModel ApplicationUserModel { get; set; }
  
    private List<ApplicationUserModel> ApplicationUsers { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        GetUserResponseDto getUserResponseDto = await AccountApiService.GetUserAsync(authenticationState.User.Identity!.Name);
        ApplicationUserModel = getUserResponseDto.ApplicationUserModel;
        
        await LoadUsersToTableAsync();
    }

    private async Task ShowUpdateUserEmailModalAsync()
    {
        ModalParameters parameters = new();
        parameters.Add("Username", ApplicationUserModel.Username);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<UpdateUserEmailModal>($"Change E-Mail - {ApplicationUserModel.Email}", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            GetUserResponseDto getUserResponseDto = await AccountApiService.GetUserAsync(ApplicationUserModel.Username);
            ApplicationUserModel = getUserResponseDto.ApplicationUserModel;

            ToastService.ShowInfo("E-Mail changed successfully.", "Success!");
        }
    }

    private async Task ShowUpdateUserUsernameModalAsync()
    {
        ModalParameters parameters = new();
        parameters.Add("Username", ApplicationUserModel.Username);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<UpdateUserUsernameModal>($"Change Username - {ApplicationUserModel.Username}", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await AccountApiService.LogoutAsync();
            NavigationManager.NavigateTo("/");
            ToastService.ShowInfo("Username changed successfully. Please login with new username.", "Success!");
        }
    }

    private async Task ShowUpdateUserPasswordModalAsync()
    {
        ModalParameters parameters = new();
        parameters.Add("Username", ApplicationUserModel.Username);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<UpdateUserPasswordModal>($"Change Password", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            ToastService.ShowInfo("Password changed successfully.", "Success!");
        }
    }

    private async Task ShowDeleteUserModalAsync()
    {
        ModalParameters parameters = new();
        parameters.Add("Username", ApplicationUserModel.Username);
        parameters.Add("Message", "Are you sure you want to delete your account?");

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<DeleteUserModal>($"Delete account", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await AccountApiService.LogoutAsync();
            NavigationManager.NavigateTo("/");
            ToastService.ShowInfo("Account deleted successfully.", "Success!");
        }
    }
    
    private async Task LoadUsersToTableAsync()
    {
        GetUsersResponseDto getUsersResponseDto = await AccountApiService.GetUsersAsync();
        List<ApplicationUserModel> allUsers = getUsersResponseDto.Result.ToList();

        AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        GetUserResponseDto getUserResponseDto = await AccountApiService.GetUserAsync(authenticationState.User.Identity!.Name);
        ApplicationUserModel applicationUserModel = getUserResponseDto.ApplicationUserModel;

        ApplicationUserModel sysadmin = allUsers.FirstOrDefault(user => user.Username == "sysadmin");
        ApplicationUserModel currentUser = allUsers.FirstOrDefault(user => user.Id == applicationUserModel.Id);

        allUsers.Remove(sysadmin);
        allUsers.Remove(currentUser);

        ApplicationUsers = allUsers;
    }
    
    private async Task ShowInsertUserModalAsync()
    {
        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<InsertUserModal>("Add new user", options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await LoadUsersToTableAsync();

            ToastService.ShowInfo("User added successfully.", "Success!");
        }
    }
    
    private async Task ShowDeleteUserModalAsync(string username)
    {
        ModalParameters parameters = new();
        parameters.Add("Username", username);
        parameters.Add("Message", $"Are you sure you want to delete user {username}?");

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<DeleteUserModal>($"Delete user - {username}", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await LoadUsersToTableAsync();

            ToastService.ShowInfo("User deleted successfully.", "Success!");
        }
    }
}