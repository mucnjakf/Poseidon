namespace Poseidon.Client.Pages.Dashboard;

public partial class DashboardPage
{
    [Inject] private IEventApiService EventApiService { get; set; }

    [Inject] private IVesselApiService VesselApiService { get; set; }

    [CascadingParameter] private IModalService ModalService { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    private List<PieChartDataItem> PieChartData { get; set; } = new();
    private int NumberOfEventsInSystem { get; set; }

    private double AverageMedianSpeedInKnots { get; set; }
    private double AverageEventDurationInHours { get; set; }
    private int NumberOfVesselsInSystem { get; set; }

    private List<IGrouping<string, EventModel>> MmsiEvents { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        GetEventsResponseDto getEventsResponseDto = await EventApiService.GetEventsAsync();
        GetVesselsResponseDto getVesselsResponseDto = await VesselApiService.GetVesselsAsync();

        LoadAllEventsPieChartAsync(getEventsResponseDto.Result.ToList());
        LoadIllegalEventsAndVessels(getEventsResponseDto.Result.ToList(), getVesselsResponseDto.Result.ToList());
    }

    private void LoadAllEventsPieChartAsync(IReadOnlyCollection<EventModel> events)
    {
        NumberOfEventsInSystem = events.Count;

        int illegalEventsAmount = events.Count(@event => @event.IsIllegal);
        int legalEventsAmount = events.Count(@event => @event.IsIllegal == false);
        int undismissedEventsAmount = events.Count(@event => @event.IsDismissed == false);

        PieChartData = new List<PieChartDataItem>
        {
            new()
            {
                Label = $"Illegal events: {illegalEventsAmount}",
                EventsAmount = illegalEventsAmount
            },
            new()
            {
                Label = $"Legal events: {legalEventsAmount}",
                EventsAmount = legalEventsAmount
            },
            new()
            {
                Label = $"Undismissed events: {undismissedEventsAmount}",
                EventsAmount = undismissedEventsAmount
            }
        };
    }

    private void LoadIllegalEventsAndVessels(IReadOnlyCollection<EventModel> events, IEnumerable<VesselModel> vessels)
    {
        AverageMedianSpeedInKnots = Math.Round(events.Average(@event => @event.MedianSpeedKnots), 4);
        AverageEventDurationInHours = events.Average(@event => @event.TotalDurationHours);

        MmsiEvents = events
            .Where(@event => @event.IsIllegal)
            .GroupBy(@event => @event.Mmsi)
            .OrderByDescending(mmsiEvent => mmsiEvent.Count())
            .Take(5)
            .ToList();

        NumberOfVesselsInSystem = vessels.Count();
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

    private class PieChartDataItem
    {
        public string Label { get; set; }
        public int EventsAmount { get; set; }
    }
}