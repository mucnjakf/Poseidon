namespace Poseidon.Client.Pages.Vessels.Modals;

public partial class VesselDetailsModal
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IEventApiService EventApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    [Parameter] public VesselModel VesselModel { get; set; }

    private List<EventModel> Events { get; set; } = new();

    private int NumberOfEvents { get; set; }
    private int NumberOfIllegalEvents { get; set; }
    private int NumberOfLegalEvents { get; set; }

    private bool ShowError { get; set; }
    private string Error { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        ShowError = false;
        Error = string.Empty;

        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync(VesselModel.Mmsi);

        if (getEventsResponseDto.Successful)
        {
            Events = getEventsResponseDto.Result.ToList();

            NumberOfEvents = Events.Count;
            NumberOfIllegalEvents = Events.Count(@event => @event.IsIllegal);
            NumberOfLegalEvents = Events.Count(@event => !@event.IsIllegal);

            return;
        }

        Error = getEventsResponseDto.Error;
        ShowError = true;
    }

    private void ShowEventDetailsModal(EventModel eventModel)
    {
        ModalParameters parameters = new();
        parameters.Add("EventModel", eventModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        ModalService.Show<EventDetailsModal>($"Event ID {eventModel.Id} - {(eventModel.IsIllegal ? "Illegal" : "Legal")} activity", parameters,
            options);
    }

    private async Task ShowEventOnMapAsync(EventModel eventModel)
    {
        await ModalInstance.CloseAsync();
        NavigationManager.NavigateTo($"/map?eventId={eventModel.Id}");
    }
}