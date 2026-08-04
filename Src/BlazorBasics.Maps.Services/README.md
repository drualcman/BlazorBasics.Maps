[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.Maps.Services?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Services)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.Maps.Services?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Services)

# Geolocation Service for Blazor

A lightweight and efficient Blazor service for interacting with the Browser's **Geolocation API**. This service uses **JS Interop** with **Lazy Loading** to ensure that JavaScript resources are only loaded when needed.

## Features

- **Always answers**: every request carries a timeout on the JavaScript side and another on the .NET side, so a browser that never calls back ends as a failed reading and not as a call that hangs.
- **Low accuracy fallback**: when a high accuracy request times out or the fix never arrives, a coarse network position is requested instead. A gps fix indoors is a fix that does not arrive.
- **A reason for every failure**: `IExtendedGeolocationService` says whether it was the permission, an unavailable position, a timeout, a browser without the api or a page that is not a secure context.
- **Lazy Initialization**: The JavaScript module is loaded only upon the first method call, and a load that failed is retried instead of being remembered as a permanent failure.
- **Memory Management**: Implements `IAsyncDisposable` to properly release JavaScript object references.

## Registration

Add the service to your Dependency Injection (DI) container in `Program.cs`:

```csharp
builder.Services.AddGeoService();
```

The defaults can be changed once for the whole application:

```csharp
builder.Services.AddGeoService(options =>
{
    options.EnableHighAccuracy = true;
    options.TimeoutMilliseconds = 15000;
    options.MaximumAgeMilliseconds = 0;
    options.FallbackToLowAccuracy = true;
    options.LowAccuracyTimeoutMilliseconds = 10000;
    options.LowAccuracyMaximumAgeMilliseconds = 60000;
});
```

## Usage

Inject `IExtendedGeolocationService` to get the coordinates and, when there are none, the reason why.

```razor
@page "/location"
@inject IExtendedGeolocationService GeolocationService

<PageTitle>Location Tracker</PageTitle>

<h1>Geolocation</h1>

@if (Reading?.IsSuccess == true)
{
    <p>Latitude: @Reading.Position.Latitude</p>
    <p>Longitude: @Reading.Position.Longitude</p>
    <p>Accuracy: @Reading.AccuracyInMeters m</p>
}
else if (Reading is not null)
{
    <p>@Message</p>
}

<button class="btn btn-primary" @onclick="GetUserLocation" disabled="@IsLocating">
    Get My Location
</button>

@code {
    GeolocationReading Reading;
    string Message;
    bool IsLocating;

    async Task GetUserLocation()
    {
        IsLocating = true;
        Reading = await GeolocationService.ReadPositionAsync();
        IsLocating = false;

        Message = Reading.FailureReason switch
        {
            GeolocationFailureReason.None => null,
            GeolocationFailureReason.PermissionDenied => "Allow this site to use your location and try again.",
            GeolocationFailureReason.PositionUnavailable => "Turn on the location services of your device and try again.",
            GeolocationFailureReason.Timeout => "Your device could not find your position. Move somewhere with a clearer sky and try again.",
            GeolocationFailureReason.InsecureContext => "This page has to be served over https to read your location.",
            GeolocationFailureReason.NotSupported => "This browser cannot read your location.",
            _ => "We could not read your location. Try again."
        };
    }
}
```

Do not gate the request behind the permission check. Before the user has answered once, the
state is `Prompt` and not `Granted`, so asking first is how an application ends up never
showing the browser prompt at all. Request the position, and read the permission afterwards
only to tell a denial apart from a device that could not find itself.

## API Reference

### `IExtendedGeolocationService`

| Method | Type | Description |
|---|---|---|
| `ReadPositionAsync(options, cancellationToken)` | `ValueTask<GeolocationReading>` | Requests the position and returns it, or the reason why there is none. Never throws for a browser side failure. |
| `ReadPermissionStateAsync(cancellationToken)` | `ValueTask<GeolocationPermissionState>` | `Granted`, `Denied`, `Prompt`, or `Unknown` where the browser does not expose the permissions api. |

### `IGeolocationService`

| Method | Type | Description |
|---|---|---|
| `GetPositionAsync()` | `ValueTask<ILatLong>` | Triggers the browser's geolocation prompt and returns the coordinates, or `null` when there are none. |
| `GetGeoLocationGrantedAsync()` | `ValueTask<bool>` | Whether the permission is already granted. `false` also means "not asked yet". |
| `DisposeAsync()` | `ValueTask` | Disposes of the JavaScript module reference. |

### `GeolocationFailureReason`

| Value | Meaning |
|---|---|
| `None` | The reading succeeded. |
| `PermissionDenied` | The user, the browser, or a `Permissions-Policy` header refused it. |
| `PositionUnavailable` | The device has no way to know where it is. |
| `Timeout` | Nothing answered within the configured budget. |
| `NotSupported` | The browser does not expose the geolocation api. Inside an iframe it also needs `allow="geolocation"`. |
| `InsecureContext` | The page is not served over https or from localhost. |
| `Unknown` | Anything else, with the browser message in `FailureMessage`. |

## Developed for BlazorBasics.Maps
You can use in conjuntion with `BlazorBasics.Maps.Google` and `BlazorBasics.Maps.Leaflet` or any other application.

## Contributing

If you encounter issues or have suggestions for improvements, please submit an issue or pull request to the repository hosting this library.
