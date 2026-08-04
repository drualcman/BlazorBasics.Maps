namespace BlazorBasics.Maps.Services;
internal class GeolocationJsReading
{
    public bool IsSuccess { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Accuracy { get; set; }
    public int FailureReason { get; set; }
    public string FailureMessage { get; set; }

    public GeolocationReading ToReading()
    {
        if (!IsSuccess)
            return GeolocationReading.FromFailure(ToFailureReason(), FailureMessage);

        try
        {
            return GeolocationReading.FromPosition(new PositionPoint(Latitude, Longitude), Accuracy);
        }
        catch (ArgumentException exception)
        {
            return GeolocationReading.FromFailure(GeolocationFailureReason.Unknown, exception.Message);
        }
    }

    GeolocationFailureReason ToFailureReason() =>
        Enum.IsDefined(typeof(GeolocationFailureReason), FailureReason)
            ? (GeolocationFailureReason)FailureReason
            : GeolocationFailureReason.Unknown;
}
