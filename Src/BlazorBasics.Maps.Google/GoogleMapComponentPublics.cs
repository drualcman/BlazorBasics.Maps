namespace BlazorBasics.Maps.Google;
#nullable enable annotations

public partial class GoogleMapComponent
{
    public async Task AddPoint(RoutePoint point)
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("addPoint",
                point.Id, point.Position.Latitude, point.Position.Longitude,
                point.Description, point.SvgIcon, point.HtmlContent);
        }
    }

    public async Task RemovePoint(string id)
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("removePoint", id);
        }
    }

    public async Task CenterMap(PositionPoint point)
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("centerMap", point.Latitude, point.Longitude);
        }
    }

    public async Task<bool> EnableCenterPin()
    {
        if (GoogleMapsModule is null)
        {
            return false;
        }

        return await GoogleMapsModule.InvokeAsync<bool>("enableCenterPin",
            dotNetRef, nameof(OnMapCenterChanged), CenterPinSvgIcon,
            GeocodeCenterOnMove, CenterChangedDebounceMilliseconds);
    }

    public async Task DisableCenterPin()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("disableCenterPin");
        }
    }

    public async Task RefreshCenter()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("refreshCenter");
        }
    }

    public async Task<PositionPoint?> GetCenter()
    {
        if (GoogleMapsModule is null)
        {
            return null;
        }

        GeocodeResponse? center = await GoogleMapsModule.InvokeAsync<GeocodeResponse?>("getCenter");
        if (center is null)
        {
            return null;
        }

        return PositionPoint.CreateFromCoordinates(center.Latitude, center.Longitude);
    }

    public async Task<MapClickEventArgs?> SearchAddress(string address)
    {
        if (GoogleMapsModule is null || string.IsNullOrWhiteSpace(address))
        {
            return null;
        }

        GeocodeResponse? found = await GoogleMapsModule.InvokeAsync<GeocodeResponse?>("geocodeAddress", address);
        if (found is null)
        {
            return null;
        }

        PositionPoint? point = PositionPoint.CreateFromCoordinates(found.Latitude, found.Longitude);
        return new MapClickEventArgs(null, found.Address, point, found.Details);
    }

    public async Task<MapClickEventArgs?> SearchAddressAndCenter(string address)
    {
        MapClickEventArgs? found = await SearchAddress(address);
        if (found?.Point is not null)
        {
            await CenterMap(new PositionPoint(found.Point.Latitude, found.Point.Longitude));
        }

        return found;
    }

    public async Task ClearMap()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("cleanMap");
        }
    }

    public async Task ShowRoute(RoutePoint startPoint, RoutePoint endPoint, string travelMode = "DRIVING", string routeId = "Route", string color = "#1a73e8")
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("showRoute",
                routeId,
                startPoint, endPoint,
                travelMode, color);
        }
    }

    public async Task ShowRouteWithWaypoints(IEnumerable<RoutePoint> points, string travelMode = "DRIVING", string routeId = "Route", string color = "#1a73e8")
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("showRouteWithWaypoints", routeId, points, travelMode, color);
        }
    }

    public async Task RemoveRoute(string routeId)
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("removeRoute", routeId);
        }
    }


    public async Task HighlightMarker(string id, string color = "#006400")
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("highlightMarker", id, color);
        }
    }

    public async Task UnhighlightMarker(string id)
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("unhighlightMarker", id);
        }
    }

    public async Task EnablePopupCloseOnClickOutside()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("enablePopupCloseOnClickOutside");
        }
    }

    public async Task DisablePopupCloseOnClickOutside()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("disablePopupCloseOnClickOutside");
        }
    }

    public async Task CloseAllPopups()
    {
        if (GoogleMapsModule is not null)
        {
            await GoogleMapsModule.InvokeVoidAsync("closeAllPopups");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (GoogleMapsModule is not null)
        {
            if (dotNetRef is not null)
            {
                dotNetRef.Dispose();
                await GoogleMapsModule.InvokeVoidAsync("disableMapClick");
                await GoogleMapsModule.InvokeVoidAsync("disableCenterPin");
            }
            await GoogleMapsModule.DisposeAsync();
        }
    }
}
