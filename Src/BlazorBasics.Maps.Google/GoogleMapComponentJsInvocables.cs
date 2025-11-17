namespace BlazorBasics.Maps.Google;

public partial class GoogleMapComponent
{
    [JSInvokable]
    public async Task OnMapClick(double? lat, double? lng, string address, AddressDetails? placeDetails, string? markerId)
    {
        if (OnClick.HasDelegate)
        {
            PositionPoint? point = PositionPoint.CreateFromCoordinates(lat, lng);
            MapClickEventArgs place = new MapClickEventArgs(markerId, address, point, placeDetails);
            await OnClick.InvokeAsync(place);
        }
    }
}
