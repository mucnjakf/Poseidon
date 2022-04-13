namespace Poseidon.Client.Pages.Map;

public partial class MapPage : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IJSRuntime JsRuntime { get; set; }

    [Inject] private IEventApiService EventApiService { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    [Inject] private EventsNotificationService EventsNotificationService { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "EventId")]
    public string EventId { get; set; }

    private List<EventModel> EventsWaitingToBeDismissedModal { get; set; } = new();

    private int NumberOfEventsLoaded { get; set; } = 100;

    private HubConnection _hubConnection;

    private bool IsSystemOnline { get; set; }

    private DotNetObjectReference<GisService> _objRef;
    private GisService _gisService;

    protected override async Task OnInitializedAsync()
    {
        _gisService = new GisService();

        _objRef = DotNetObjectReference.Create(_gisService);

        _gisService.OpenEventDetailsModal += OnOpenEventDetailsModalAsync;
        _gisService.NotifyLatLng += OnNotifyLatLng;

        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();

        List<EventModel> latestEvents = getEventsResponseDto.Result.OrderByDescending(@event => @event.Id).Take(100).ToList();
        List<EventModel> eventsToBeDismissed = getEventsResponseDto.Result.Where(@event => @event.IsDismissed == false).ToList();
        EventsWaitingToBeDismissedModal = eventsToBeDismissed;

        List<EventModel> events = latestEvents.Except(eventsToBeDismissed).ToList();

        events.AddRange(eventsToBeDismissed);

        await JsRuntime.InvokeAsync<object>("loadLatestEvents", events);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (EventId is not null)
        {
            GetEventResponseDto getEventResponseDto = await EventApiService.GetEventAsync(int.Parse(EventId));

            if (getEventResponseDto.Successful)
            {
                EventModel eventModel = getEventResponseDto.EventModel;
                await JsRuntime.InvokeAsync<object>("loadExistingEvent", eventModel);
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeAsync<object>("leafletJsFunctions.initialize", _objRef);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task StartSystemAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/event-hub"))
            .Build();

        _hubConnection.On<EventModel>("ReceiveEvent", async eventModel =>
        {
            await JsRuntime.InvokeAsync<object>("loadNewEvent", eventModel);

            EventsWaitingToBeDismissedModal.Add(eventModel);
            StateHasChanged();

            ToastService.ShowInfo($"System received new {(eventModel.IsIllegal ? "illegal" : "legal")} event.", "New Event!");
        });

        await _hubConnection.StartAsync();

        await EventApiService.ReceiveEventsAsync(true);

        IsSystemOnline = true;
        EventsNotificationService.SetIsReceivingEvents(true);
        StateHasChanged();

        ToastService.ShowInfo("System is starting to receive new events.", "System Started!");
    }

    private async Task StopSystemAsync()
    {
        if (_hubConnection is not null)
        {
            await EventApiService.ReceiveEventsAsync(false);

            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();

            IsSystemOnline = false;
            EventsNotificationService.SetIsReceivingEvents(false);
            StateHasChanged();

            ToastService.ShowInfo("System stops receiving new events.", "System Stopped!");
        }
    }

    private async Task OnOpenEventDetailsModalAsync()
    {
        await InvokeAsync(StateHasChanged);

        if (_gisService.EventModel is null)
        {
            return;
        }

        ModalParameters parameters = new();
        parameters.Add("EventModel", _gisService.EventModel);

        ModalOptions options = new() { Animation = ModalAnimation.FadeInOut(0.3) };

        IModalReference modalReference = ModalService.Show<EventDetailsModal>(
            $"Event ID {_gisService.EventModel.Id} - {(_gisService.EventModel.IsIllegal ? "Illegal" : "Legal")} activity",
            parameters, options);

        ModalResult result = await modalReference.Result;

        if (!result.Cancelled)
        {
            GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();
            EventsWaitingToBeDismissedModal = getEventsResponseDto.Result.Where(@event => @event.IsDismissed == false).ToList();
            StateHasChanged();
        }
    }

    private async Task OnNotifyLatLng()
    {
        await InvokeAsync(StateHasChanged);
    }

    private async Task FlyToEvent(string latitude, string longitude)
        => await JsRuntime.InvokeAsync<object>("flyToEvent", latitude, longitude);

    private async Task OnEventsLoadedChangeAsync(dynamic value)
    {
        int numberOfEvents = value;

        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();

        List<EventModel> latestEvents = getEventsResponseDto.Result
            .Where(@event => @event.IsDismissed)
            .OrderByDescending(@event => @event.Id)
            .Take(numberOfEvents)
            .ToList();

        List<EventModel> undismissedEvents = getEventsResponseDto.Result
            .Where(@event => @event.IsDismissed == false)
            .ToList();

        latestEvents.AddRange(undismissedEvents);

        await JsRuntime.InvokeAsync<object>("loadLatestEvents", latestEvents);

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

    public void Dispose()
    {
        _gisService.OpenEventDetailsModal -= OnOpenEventDetailsModalAsync;
        _gisService.NotifyLatLng -= OnNotifyLatLng;
        _objRef?.Dispose();
    }
}