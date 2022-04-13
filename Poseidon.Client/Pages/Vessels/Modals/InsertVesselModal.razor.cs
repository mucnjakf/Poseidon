namespace Poseidon.Client.Pages.Vessels.Modals;

public partial class InsertVesselModal
{
    [Inject] private IValidator<InsertVesselRequestDto> InsertVesselRequestDtoValidator { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    private InsertVesselRequestDto InsertVesselRequestDto { get; } = new();

    private bool ShowErrors { get; set; }

    private List<string> Errors { get; set; } = new();

    private async Task HandleInsertVesselAsync()
    {
        ShowErrors = false;
        Errors.Clear();

        if (FormIsValid())
        {
            InsertVesselResponseDto response = await VesselApiService.InsertVesselAsync(InsertVesselRequestDto);

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
        ValidationResult validationResult = InsertVesselRequestDtoValidator.Validate(InsertVesselRequestDto);

        if (validationResult.IsValid) return true;

        Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

        ShowErrors = true;

        return false;
    }
}