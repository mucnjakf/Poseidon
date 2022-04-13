using System.Net.WebSockets;

namespace Poseidon.Client.Pages.Events;

public partial class EventsPage
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IEventApiService EventApiService { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    private List<EventModel> Events { get; set; }

    protected override async Task OnInitializedAsync()
    {
        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();
        Events = getEventsResponseDto.Result.ToList();
    }

    private void ShowEventDetailsModal(EventModel eventModel)
    {
        ModalParameters parameters = new();
        parameters.Add("EventModel", eventModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        ModalService.Show<EventDetailsModal>($"Event ID {eventModel.Id} - {(eventModel.IsIllegal ? "Illegal" : "Legal")} activity", parameters,
            options);
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

    private void ShowEventOnMap(EventModel eventModel) => NavigationManager.NavigateTo($"/map?eventId={eventModel.Id}");
}