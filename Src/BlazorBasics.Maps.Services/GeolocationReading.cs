namespace BlazorBasics.Maps.Services;
public class GeolocationReading
{
    public bool IsSuccess { get; }
    public ILatLong Position { get; }
    public double AccuracyInMeters { get; }
    public GeolocationFailureReason FailureReason { get; }
    public string FailureMessage { get; }

    GeolocationReading(bool isSuccess, ILatLong position, double accuracyInMeters,
        GeolocationFailureReason failureReason, string failureMessage)
    {
        IsSuccess = isSuccess;
        Position = position;
        AccuracyInMeters = accuracyInMeters;
        FailureReason = failureReason;
        FailureMessage = failureMessage;
    }

    public static GeolocationReading FromPosition(ILatLong position, double accuracyInMeters) =>
        new(true, position, accuracyInMeters, GeolocationFailureReason.None, null);

    public static GeolocationReading FromFailure(GeolocationFailureReason failureReason, string failureMessage) =>
        new(false, null, 0, failureReason, failureMessage);

    public override string ToString() =>
        IsSuccess
            ? $"{Position} (±{AccuracyInMeters} m)"
            : $"{FailureReason}: {FailureMessage}";
}
