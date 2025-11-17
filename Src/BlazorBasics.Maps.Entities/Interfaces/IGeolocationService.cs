namespace BlazorBasics.Maps.Entities.Interfaces;
public interface IGeolocationService
{
    ValueTask<ILatLong> GetPositionAsync();
    ValueTask<bool> GetGeoLocationGrantedAsync();
}
