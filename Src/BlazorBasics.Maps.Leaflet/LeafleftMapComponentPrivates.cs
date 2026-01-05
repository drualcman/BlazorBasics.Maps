namespace BlazorBasics.Maps.Leaflet;

public partial class LeafleftMapComponent : IAsyncDisposable
{
    DotNetObjectReference<LeafleftMapComponent> ObjRef;
    string MapId = $"map-{Guid.NewGuid()}";
    bool IsMapReady = false;

    private string GetIcon(Icon icon)
    {
        string useIcon = icon switch
        {
            Icon.DRON => "drone",
            Icon.HOME => "home",
            Icon.DESTINATION => "destination",
            _ => "marker-icon"
        };
        return useIcon;
    }

    private string GetIconUrl(string iconUrl)
    {
        if (iconUrl.Contains("http"))
            return iconUrl;
        else
            return $"./{ContentHelper.ContentPath}/css/images/{iconUrl}.png";
    }
}