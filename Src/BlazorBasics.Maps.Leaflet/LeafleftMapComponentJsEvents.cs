namespace BlazorBasics.Maps.Leaflet;

public partial class LeafleftMapComponent : IAsyncDisposable
{
    [JSInvokable]
    public async Task OnDragend(DragendMarkerInternalEventArgs e)
    {
        if (OnDragendAsync.HasDelegate)
        {
            var publicArgs = new DragendMarkerEventArgs
            {
                MarkerId = e.MarkerId,
                Point = e.Point
            };
            await OnDragendAsync.InvokeAsync(publicArgs);
        }
    }

    [JSInvokable]
    public async Task OnMapClick(double? lat, double? lng, string address, AddressDetails? placeDetails, string? markerId)
    {
        if (OnMapClickAsync.HasDelegate)
        {
            PositionPoint point = PositionPoint.CreateFromCoordinates(lat, lng);
            MapClickEventArgs place = new MapClickEventArgs(markerId, address, point, placeDetails);
            await OnMapClickAsync.InvokeAsync(place);
        }
    }
}