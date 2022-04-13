namespace Poseidon.Client.Pages.Events.Modals;

public partial class EventDetailsModal
{
    [Inject] private IJSRuntime JsRuntime { get; set; }

    [Inject] private IEventApiService EventApiService { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    [Parameter] public EventModel EventModel { get; set; }

    private async Task HandleDismissEventAsync()
    {
        await EventApiService.DismissEventAsync(EventModel.Id);

        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();

        List<EventModel> latestEvents = getEventsResponseDto.Result.OrderByDescending(@event => @event.Id).Take(100).ToList();
        List<EventModel> eventsWaitingToBeDismissed = getEventsResponseDto.Result.Where(@event => @event.IsDismissed == false).ToList();

        List<EventModel> events = latestEvents.Except(eventsWaitingToBeDismissed).ToList();

        events.AddRange(eventsWaitingToBeDismissed);

        await JsRuntime.InvokeAsync<object>("loadLatestEvents", events);

        await ModalInstance.CloseAsync();

        StateHasChanged();
    }

    private async Task ShowVesselDetailsModalAsync(string mmsi)
    {
        GetVesselResponseDto getVesselResponseDto = await VesselApiService.GetVesselAsync(mmsi);

        if (getVesselResponseDto.Successful)
        {
            ModalParameters parameters = new();
            parameters.Add("VesselModel", getVesselResponseDto.VesselModel);

            ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

            ModalService.Show<VesselDetailsModal>($"Vessel details - {getVesselResponseDto.VesselModel.Name}", parameters, options);
            return;
        }

        ToastService.ShowError(getVesselResponseDto.Error, "Error!");
    }
}