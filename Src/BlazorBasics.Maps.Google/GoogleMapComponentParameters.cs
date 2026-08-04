namespace BlazorBasics.Maps.Google;

public partial class GoogleMapComponent
{
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Parameter] public string ApiKey { get; set; } = string.Empty;
    [Parameter] public string MapId { get; set; } = string.Empty;
    [Parameter] public int StartZoomLevel { get; set; } = 13;
    [Parameter] public bool ClosePopupWhenClickOnMap { get; set; } = true;
    [Parameter] public bool ClosePopupWhenClickOther { get; set; }
    [Parameter] public EventCallback OnMapReady { get; set; }
    [Parameter] public EventCallback<MapClickEventArgs> OnClick { get; set; }
    [Parameter] public bool ShowCenterPin { get; set; }
    [Parameter] public string CenterPinSvgIcon { get; set; } = string.Empty;
    [Parameter] public bool GeocodeCenterOnMove { get; set; } = true;
    [Parameter] public int CenterChangedDebounceMilliseconds { get; set; } = 300;
    [Parameter] public EventCallback<MapClickEventArgs> OnCenterChanged { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }
}
