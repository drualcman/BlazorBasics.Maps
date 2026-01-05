[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.Maps.Entities?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Entities)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.Maps.Entities?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.Maps.Entities)


# BlazorBasics.Maps Library Documentation

## Overview

The `BlazorBasics.Maps` library to use common dependencies in `BlazorBasics.Maps.Google` and `BlazorBasics.Maps.Leaflet`

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
      - Details: An IAddressDetails object with the address components broken down.

### Interfaces

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
## Contributing

If you encounter issues or have suggestions for improvements, please submit an issue or pull request to the repository hosting this library.