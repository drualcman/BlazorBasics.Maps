[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.Maps.Leaflet?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Leaflet)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.Maps.Leaflet?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Leaflet)

# BlazorBasics.Maps.Leaflet Library Documentation

## Overview

The `BlazorBasics.Maps.Leaflet` library is a Blazor component designed to integrate Leaflet Javascript functionality into Blazor applications. It provides a simple and reusable way to display a Leaflet|OpenStreet Map, add markers (points) and interact with the map programmatically. The library uses JavaScript interop to communicate with the Leaflet Maps JavaScript API and supports common map operations such as adding points, move icon and get coordinates.

This library is ideal for Blazor developers who need to embed interactive maps with custom markers their web applications. It abstracts the complexity of Leaflet Maps JavaScript API interactions while providing a clean C# interface.

## Prerequisites

To use this library, you need:

- A Blazor application (Server or WebAssembly).

## Installation

1. **Install the NuGet Package**:
   - Install the `BlazorBasics.Maps.Leaflet` NuGet package in your Blazor project using the Package Manager or the .NET CLI:
     ```bash
     dotnet add package BlazorBasics.Maps.Leaflet
     ```

2. **Add CSS Styling**:
   - Ensure the map container has appropriate styling. For example, add the following CSS to your stylesheet (e.g., `wwwroot/css/site.css`):

     ```css
     .map {
         height: 400px; /* Adjust height as needed */
         width: 100%;
     }
     ```

3. **Map service**:
   - Add the `MapService` to your dependencies.
   ```csharp
   var builder = WebAssemblyHostBuilder.CreateDefault(args);
   [...]
   builder.Services.AddMapsService();
   [...]
   ```

4. **Include the Component in Your Razor Page**:
   - Add the `LeafleftMapComponent` to your Razor page.

## Usage

### Basic Setup

To display a Leaflet Map, include the `LeafleftMapComponent` in your Razor. Specify an `OnMapCreatedAsync` callback to execute code once the map is initialized.

```razor
@using BlazorBasics.Maps.Leaflet

  <LeafleftMapComponent 
        class="map" 
        OriginalPoint="CompanyPoint" 
        OnMapCreatedAsync=OnMapCreatedAsync 
        ZoomLevel="20" />
         

@code {
    LeafleftMapComponent Map;
    ILatLong CompanyPoint;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await CompanyRepository.GetCompanyData();
            Company = CompanyRepository.Company;
            CompanyPoint = new LatLong(Company.Address.Location.Lat, Company.Address.Location.Long);
            await InvokeAsync(StateHasChanged);
        }
    }

    
    async Task OnMapCreatedAsync(LeafleftMapComponent map)
    {
        Map = map;
        int markId = await Map.AddMarkerAsync(CompanyPoint, OwnLocalizer[nameof(MapButtonComponentContent.WeAreHere)], OwnLocalizer[nameof(MapButtonComponentContent.ComeAndVisit)], Icon.HOME);
        await Map.MoveMarketAsync(markId, CompanyPoint);
        await Map.SetViewAsync(CompanyPoint);
    }
}
```

### Component Parameters

| Parameter          | Type                                    | Description                                            |
|--------------------|-----------------------------------------|--------------------------------------------------------|
| `OriginalPoint`    | `ILatLong`                              | Your initial position in the map.                      |
| `ZoomLevel`        | `byte`                                  | The zoom level to fet the view. Default valur 19       |
| `OnMapCreatedAsync`| `EventCallback<LeafleftMapComponent>`   | Call back return a objet with the current map created. |
| `OnMapClickAsync`  | `EventCallback<MapClickEventArgs>`      | A callback that is invoked when the map is clicked.    |
| `OnDragendAsync`   | `EventCallback<DragendMarkerEventArgs>` | A callback that is invoked when the marker is moved.   |

### Models

The library includes two main models:

- **`AddressDetails`**:
  - Represents full address.
  - Implement from `IAddressDetails`
  - Properties:
    - StreetNumber
    - Route
    - Neighborhood
    - Locality: Typically refers to the city or town name.
    - AdministrativeArea: Typically refers to the state, province, or region.
    - PostalCode
    - Country  

- **`PositionPoint`**:
  - Represents a geographic coordinate with `Latitude` and `Longitude`.
  - Implement from `ILatLong` and `IEquatable<PositionPoint>`
  - Validates coordinates to ensure they are within valid ranges (`Latitude`: -90 to 90, `Longitude`: -180 to 180).
  - Example:
    ```csharp
    var point = new PositionPoint(latitude: 37.7749f, longitude: -122.4194f);
    ```
  
- **`MapClickEventArgs`**:
    - Event arguments for the OnClick callback. Contains information about the point where the click was made.
    - Properties:
      - MarkerId: The marker ID if one was clicked; otherwise, null.
      - Point: A ILatLong object with the click coordinates.
      - Address: A string containing the address formatted at the point of the click (reverse geocoding).
      - Details: An `IAddressDetails` object with the address components broken down.

- **`IAddressDetails`**
    - Contains the broken-down components of an address obtained from reverse geocoding.
    - Properties: 
      - StreetNumber
      - Route
      - Neighborhood
      - Locality: (city)
      - AdministrativeArea: (state/province)
      - Postal Code
      - Country

- **`ILatLong`**
    - Contains latitude and longitude.
    - Properties: 
      - Latitude
      - Longitude

### Available Methods

The `LeafleftMapComponent` provides the following methods to interact with the map:

1. **`AddMarkerAsync(ILatLong point, string title, string description, string iconUrl)`**:
   - Adds a marker to the map at the specified position with custom properties.
   - Example:
     ```csharp
     await mapComponent.AddMarkerAsync(new LatLong(37.7749f, -122.4194f), "San Francisco", Big City", "home");
     ```

2. **`AddMarkerAsync(ILatLong point, string title, string description, Icon icon)`**:
   - Adds a marker to the map at the specified position with custom properties.
   - Example:
     ```csharp
     await mapComponent.AddMarkerAsync(new LatLong(37.7749f, -122.4194f), "San Francisco", "Big City", Icon.PIN);
     ```

3. **`AddMarkerAsync(ILatLong point, string title, string description)`**:
   - Adds a marker to the map at the specified position with custom properties.
   - Example:
     ```csharp
     await mapComponent.AddMarkerAsync(new LatLong(37.7749f, -122.4194f), "San Francisco", "Big City");
     ```

4. **`AddDraggableMarkerAsync(ILatLong point, string title, string description, string iconUrl)`**:
   - Adds a marker to the map at the specified position with custom properties who can be drag and move to other position.
   - Example:
     ```csharp
     await mapComponent.AddDraggableMarkerAsync(new LatLong(37.7749f, -122.4194f), "San Francisco", Big City", "home");
     ```

5. **`AddDraggableMarkerAsync(ILatLong point, string title, string description, Icon icon)`**:
   - Adds a marker to the map at the specified position with custom properties who can be drag and move to other position.
   - Example:
     ```csharp
     await mapComponent.AddDraggableMarkerAsync(new LatLong(37.7749f, -122.4194f), "San Francisco", Big City",  Icon.PIN);
     ```

6. **`RemoveMarkersAsync()`**:
   - Delete all markets points.
   - Example:
     ```csharp
     await mapComponent.RemoveMarkersAsync();
     ```

7. **`DrawCircleAsync(ILatLong point, string color, string fillColor, double fillOpacity, double radius)`**:
   - Draw a circle arrount the point.
   - Example:
     ```csharp
     await mapComponent.DrawCircleAsync(new LatLong(37.7749f, -122.4194f), "red", 1, 5);
     ```

8. **`DeleteMap()`**:
   - Removes the map from the page.
   - Example:
     ```csharp
     await mapComponent.DeleteMap();
     ```

9. **`MoveMarketAsync(int markerId, ILatLong newPosition)`**:
   - Move selected marker ID to the new position.
   - Example:
     ```csharp
     await mapComponent.RemoveRoute(1, new LatLong(37.7749f, -122.4194f));
     ```

10. **`SetPopupMarkerContent(int markerId, string content)`**:
   - Set a content for the popup on the selected MArker ID.
   - Example:
     ```csharp
     await mapComponent.SetPopupMarkerContent(1, "Small city"));
     ```

11. **`GetDistanceInMettersBetween(ILatLong origin, ILatLong destination)`**:
   - Get the distance in metter between 2 points.
   - Example:
     ```csharp
     await mapComponent.GetDistanceInMettersBetween(new LatLong(37.7749f, -122.4194f), new LatLong(36.7749f, -122.4194f)));
     ```

## Contributing

If you encounter issues or have suggestions for improvements, please submit an issue or pull request to the repository hosting this library.