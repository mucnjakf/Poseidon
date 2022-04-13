namespace Poseidon.Client.Pages.Vessels;

public partial class VesselsPage
{
    [Inject] private IVesselApiService VesselApiService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    private List<VesselModel> Vessels { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadVesselsToTableAsync();
    }

    private async Task LoadVesselsToTableAsync()
    {
        GetVesselsResponseDto getVesselsResponseDto = await VesselApiService.GetVesselsAsync();
        Vessels = getVesselsResponseDto.Result.ToList();
    }

    private void ShowVesselDetailsModal(VesselModel vesselModel)
    {
        ModalParameters parameters = new();
        parameters.Add("VesselModel", vesselModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        ModalService.Show<VesselDetailsModal>($"Vessel details - {vesselModel.Name}", parameters, options);
    }
    
    private async Task ShowInsertVesselModalAsync()
    {
        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<InsertVesselModal>("Add new vessel", options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await LoadVesselsToTableAsync();

            ToastService.ShowInfo("User added successfully.", "Success!");
        }
    }

    private async Task ShowUpdateVesselModalAsync(VesselModel vesselModel)
    {
        ModalParameters parameters = new();
        parameters.Add("VesselModel", vesselModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<UpdateVesselModal>($"Update vessel - {vesselModel.Name}", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            ToastService.ShowInfo("Vessel updated successfully.", "Success!");
        }
    }

    private async Task ShowDeleteVesselModalAsync(VesselModel vesselModel)
    {
        ModalParameters parameters = new();
        parameters.Add("VesselModel", vesselModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modal = ModalService.Show<DeleteVesselModal>($"Delete vessel - {vesselModel.Name}", parameters, options);
        ModalResult result = await modal.Result;

        if (!result.Cancelled)
        {
            await LoadVesselsToTableAsync();

            ToastService.ShowInfo("Vessel deleted successfully.", "Success!");
        }
    }
}