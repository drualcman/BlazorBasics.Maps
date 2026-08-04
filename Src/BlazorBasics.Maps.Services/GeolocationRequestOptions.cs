namespace BlazorBasics.Maps.Services;
public class GeolocationRequestOptions
{
    public bool EnableHighAccuracy { get; set; } = true;
    public int TimeoutMilliseconds { get; set; } = 15000;
    public int MaximumAgeMilliseconds { get; set; } = 0;
    public bool FallbackToLowAccuracy { get; set; } = true;
    public int LowAccuracyTimeoutMilliseconds { get; set; } = 10000;
    public int LowAccuracyMaximumAgeMilliseconds { get; set; } = 60000;

    public GeolocationRequestOptions Clone() => new()
    {
        EnableHighAccuracy = EnableHighAccuracy,
        TimeoutMilliseconds = TimeoutMilliseconds,
        MaximumAgeMilliseconds = MaximumAgeMilliseconds,
        FallbackToLowAccuracy = FallbackToLowAccuracy,
        LowAccuracyTimeoutMilliseconds = LowAccuracyTimeoutMilliseconds,
        LowAccuracyMaximumAgeMilliseconds = LowAccuracyMaximumAgeMilliseconds
    };

    internal int TotalBudgetMilliseconds =>
        TimeoutMilliseconds
        + (EnableHighAccuracy && FallbackToLowAccuracy ? LowAccuracyTimeoutMilliseconds : 0);
}
