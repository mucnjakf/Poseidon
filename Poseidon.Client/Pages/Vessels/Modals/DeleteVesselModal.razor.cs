namespace Poseidon.Client.Pages.Vessels.Modals;

public partial class DeleteVesselModal
{
    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public VesselModel VesselModel { get; set; }

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleDeleteVesselAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        DeleteVesselResponseDto response = await VesselApiService.DeleteVesselAsync(VesselModel.Id);

        if (response.Successful)
        {
            await ModalInstance.CloseAsync();
            return;
        }

        Errors = response.Errors.ToList();
        ShowErrors = true;
    }
}