namespace Poseidon.Client.Services;

public class EventsNotificationService
{
    public bool IsReceivingEvents { get; set; }

    public event Action OnChange;

    public void SetIsReceivingEvents(bool isReceivingEvents)
    {
        IsReceivingEvents = isReceivingEvents;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}