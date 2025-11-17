namespace BlazorBasics.Maps.Services;
internal class GeolocationService : IAsyncDisposable, IGeolocationService
{
    private readonly Lazy<Task<IJSObjectReference>> ModuleTask;

    public GeolocationService(IJSRuntime jsRuntime)
    {
        ModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", $"./{ContentHelper.ContentPath}/geolocation.js").AsTask());
    }

    public async ValueTask<ILatLong> GetPositionAsync()
    {
        var module = await ModuleTask.Value;
        PositionPoint postition = default;
        try
        {
            postition = await module.InvokeAsync<PositionPoint>("getPositionAsync");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetPosition: {ex.Message}");
        }
        return postition;
    }

    public async ValueTask<bool> GetGeoLocationGrantedAsync()
    {
        var module = await ModuleTask.Value;
        bool result;
        try
        {
            result = await module.InvokeAsync<bool>("checkGeolocationPermission");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"IsGeoLocationGranted: {ex.Message}");
            result = false;
        }
        return result;

    }

    public async ValueTask DisposeAsync()
    {
        if (ModuleTask.IsValueCreated)
        {
            var module = await ModuleTask.Value;
            await module.DisposeAsync();
        }
    }
}
