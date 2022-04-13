namespace Poseidon.Client.Pages.Components;

public partial class NavMenuComponent
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IAccountApiService AccountApiService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    [Inject] private EventsNotificationService EventsNotificationService { get; set; }

    private bool ShouldAllowNavigation { get; set; } = true;

    protected override void OnInitialized() => EventsNotificationService.OnChange += OnStateServiceChange;

    private void OnStateServiceChange() => ShouldAllowNavigation = !EventsNotificationService.IsReceivingEvents;

    private void NavigateTo(string uri)
    {
        if (uri == "/hangfire")
        {
            NavigationManager.NavigateTo(uri, true);
            return;
        }
        
        if (ShouldAllowNavigation)
        {
            NavigationManager.NavigateTo(uri);
            return;
        }

        ToastService.ShowError("Stop receiving events before navigating.", "Important!");
    }

    private async Task HandleLogoutAsync()
    {
        await AccountApiService.LogoutAsync();
        NavigationManager.NavigateTo("/");
        ToastService.ShowInfo("Logged out successfully.", "Success!");
    }
}