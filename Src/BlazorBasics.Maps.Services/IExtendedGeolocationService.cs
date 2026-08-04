namespace BlazorBasics.Maps.Services;
public interface IExtendedGeolocationService : IGeolocationService
{
    ValueTask<GeolocationReading> ReadPositionAsync(GeolocationRequestOptions options = null,
        CancellationToken cancellationToken = default);

    ValueTask<GeolocationPermissionState> ReadPermissionStateAsync(
        CancellationToken cancellationToken = default);
}
