namespace Poseidon.Client.Pages.Vessels.Modals;

public partial class UpdateVesselModal
{
    [Inject] private IValidator<UpdateVesselRequestDto> UpdateVesselRequestDtoValidator { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [Parameter] public VesselModel VesselModel { get; set; }
    
    private UpdateVesselRequestDto UpdateVesselRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleUpdateVesselAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        UpdateVesselRequestDto.Mmsi = VesselModel.Mmsi;
        UpdateVesselRequestDto.Name = VesselModel.Name;
        UpdateVesselRequestDto.Callsign = VesselModel.Callsign;
        UpdateVesselRequestDto.Flag = VesselModel.Flag;
        UpdateVesselRequestDto.Imo = VesselModel.Imo;
        
        if (FormIsValid())
        {
            UpdateVesselResponseDto response = await VesselApiService.UpdateVesselAsync(VesselModel.Id, UpdateVesselRequestDto);

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
        ValidationResult validationResult = UpdateVesselRequestDtoValidator.Validate(UpdateVesselRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}