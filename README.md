# BlazorBasics.Maps – Complete Project (Google Maps + Leaflet)

This repository contains a set of Blazor libraries for integrating interactive maps into .NET applications (Blazor Server and WebAssembly), offering two implementations with the same C# API:

`BlazorBasics.Maps.Google` -> Full integration with Google Maps (markers, routes with directions, modes of transport, directional arrows, clicks with reverse geocoding, etc.).

`BlazorBasics.Maps.Leaflet` -> Integration with Leaflet + OpenStreetMap (free, no paid API key required), with custom markers, drag and drop, and click events.

Both libraries share the exact same models and contracts thanks to the common `BlazorBasics.Maps.Entities` library, allowing you to switch map providers (Google ↔ Leaflet) with a single component and NuGet package change, without touching the business code.

## Common Features (Identical API in both providers)

- Strongly typed and validated models (`PositionPoint`, `AddressDetails`, `RoutePoint`…)
- Click events with coordinates + full address (reverse geocoding when supported by the provider)
- Add/remove markers with custom SVG icons and HTML info windows
- Center map, clear map
- Highlight/unhighlight markers
- `OnMapReady` / `OnMapCreatedAsync` and `OnClick` callbacks

## Google Maps Exclusive Features

- Route calculation and drawing (DRIVING, WALKING, BICYCLING, TRANSIT)
- Routes with intermediate waypoints
- Configurable directional arrows on routes
- Custom styles using Google Cloud Map ID

## Leaflet Exclusive Features

- 100% free (uses tiles from OpenStreetMap or other providers)
- Draggable markers with event `OnDragend`
- Injectable `IMapService` for use outside of components
- Very lightweight and without external paid dependencies

### When to use each one?

+----------------------------------------------------------------+-------------------+
| Situation                                                      | Recommended       |
+----------------------------------------------------------------+-------------------+
| You need real routes and modes of transport                    | Google Maps       |
+----------------------------------------------------------------+-------------------+
| You want to avoid API costs from Google                        | Leaflet / OSM     |
+----------------------------------------------------------------+-------------------+
| Your client already pays for Google Maps Platform              | Google Maps       |
+----------------------------------------------------------------+-------------------+
| Open-source project or limited budget Limited                  | Leaflet           |
+----------------------------------------------------------------+-------------------+
| You need maximum customization of styles and arrows on routes  | Google Maps       |
+----------------------------------------------------------------+-------------------+

# Contributions
If you find bugs or want to add new features (support for polygons, layers, clustering, etc.), you are welcome to open issues or pull requests in this repository.