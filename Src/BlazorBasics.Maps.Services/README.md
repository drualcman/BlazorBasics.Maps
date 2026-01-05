[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.Maps.Services?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Services)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.Maps.Services?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Services)

# Geolocation Service for Blazor

A lightweight and efficient Blazor service for interacting with the Browser's **Geolocation API**. This service uses **JS Interop** with **Lazy Loading** to ensure that JavaScript resources are only loaded when needed.

## Features

- **Lazy Initialization**: The JavaScript module is loaded only upon the first method call.
- **Permission Handling**: Includes a built-in check to verify if the user has granted geolocation permissions.
- **Memory Management**: Implements `IAsyncDisposable` to properly release JavaScript object references.
- **Robust Error Handling**: Internal try-catch blocks prevent JS runtime errors from crashing the .NET process, logging issues to the browser console instead.

## Registration
Add the service to your Dependency Injection (DI) container in `Program.cs`:

```csharp
builder.Services.AddGeoService();
```

## Usage
Inject the `IGeolocationService` into your Razor components to retrieve the user's coordinates.

```Razor
@page "/location"
@inject IGeolocationService GeolocationService

<PageTitle>Location Tracker</PageTitle>

<h1>Geolocation</h1>

@if (currentPoint != null)
{
    <p>Latitude: @currentPoint.Latitude</p>
    <p>Longitude: @currentPoint.Longitude</p>
}

<button class="btn btn-primary" @onclick="GetUserLocation">
    Get My Location
</button>

@code {
    private ILatLong? currentPoint;

    private async Task GetUserLocation()
    {
        bool isGranted = await GeolocationService.GetGeoLocationGrantedAsync();
        
        if (isGranted)
        {
            currentPoint = await GeolocationService.GetPositionAsync();
        }
        else
        {
            // Handle permission denied or prompt user
            Console.WriteLine("Location access denied.");
        }
    }
}
```

## API Reference

Method|Return Type|Description

| Method       | Type                               | Description                                                                   |
|-----------------|------------------------------------|----------------------------------------------------------------------------|
|GetPositionAsync()|`ValueTask<ILatLong>`|Triggers the browser's geolocation prompt and returns the coordinates.|
|GetGeoLocationGrantedAsync()|`ValueTask<bool>`|Checks if the user has already granted geolocation permissions.|
|DisposeAsync()|`ValueTask`|Disposes of the JavaScript module reference.|

## Developed for BlazorBasics.Maps
You can use in conjuntion with `BlazorBasics.Maps.Google` and `BlazorBasics.Maps.Leaflet` or any other application.

## Contributing

If you encounter issues or have suggestions for improvements, please submit an issue or pull request to the repository hosting this library.

