namespace BlazorBasics.Maps.Services;
public enum GeolocationFailureReason
{
    None = 0,
    PermissionDenied = 1,
    PositionUnavailable = 2,
    Timeout = 3,
    NotSupported = 4,
    InsecureContext = 5,
    Unknown = 6
}
