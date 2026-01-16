namespace BlazorBasics.Maps.Google;

public partial class GoogleMapComponent
{
    protected override void OnInitialized()
    {
        dotNetRef = DotNetObjectReference.Create(this);
    }

    protected override void OnParametersSet()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new ArgumentException("API Key is required to load Google Maps.", nameof(ApiKey));
        if (string.IsNullOrWhiteSpace(MapId))
            throw new ArgumentException("MapId is required to identify the map instance.", nameof(MapId));

        if (Attributes == null)
        {
            Attributes = new Dictionary<string, object>();
        }

        string existingClasses = string.Empty;
        if (Attributes.TryGetValue("class", out var cssClassObj))
        {
            existingClasses = cssClassObj?.ToString() ?? string.Empty;
        }

        List<string> classList = existingClasses
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        if (!classList.Contains("map"))
        {
            classList.Add("map"); // Add at the end, preserving original order
        }

        Attributes["class"] = string.Join(" ", classList);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !string.IsNullOrWhiteSpace(ApiKey))
        {
            // Import the JS module from your assembly
            GoogleMapsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", $"./{ContentHelper.ContentPath}/loadGoogleMaps.js?v={DateTime.Now.Ticks}");

            bool loaded = await GoogleMapsModule.InvokeAsync<bool>("load", ApiKey, ScriptId);
            if (loaded)
            {
                await GoogleMapsModule.InvokeVoidAsync("initMap", DOMMapId, MapId, ClosePopupWhenClickOther, StartZoomLevel);
                if (OnClick.HasDelegate)
                {
                    await GoogleMapsModule.InvokeVoidAsync("enableMapClick", dotNetRef, nameof(OnMapClick));
                }
                if (OnMapReady.HasDelegate)
                {
                    await OnMapReady.InvokeAsync();
                }
                if (ClosePopupWhenClickOnMap)
                {
                    await EnablePopupCloseOnClickOutside();
                }
            }
        }
    }
}
