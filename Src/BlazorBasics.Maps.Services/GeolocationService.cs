namespace BlazorBasics.Maps.Services;

internal class GeolocationService : IExtendedGeolocationService, IAsyncDisposable
{
    const int MODULE_LOAD_TIMEOUT_MILLISECONDS = 20000;
    const int PERMISSION_TIMEOUT_MILLISECONDS = 5000;

    // On top of the budget the javascript side already enforces, so a browser that never
    // answers ends as a cancelled call and not as a task nobody ever completes.
    const int INTEROP_MARGIN_MILLISECONDS = 5000;

    readonly IJSRuntime JsRuntime;
    readonly GeolocationRequestOptions DefaultOptions;
    readonly SemaphoreSlim ModuleLock = new(1, 1);

    IJSObjectReference Module;
    bool IsDisposed;

    public GeolocationService(IJSRuntime jsRuntime, GeolocationRequestOptions defaultOptions)
    {
        JsRuntime = jsRuntime;
        DefaultOptions = defaultOptions ?? new GeolocationRequestOptions();
    }

    public async ValueTask<GeolocationReading> ReadPositionAsync(GeolocationRequestOptions options = null,
        CancellationToken cancellationToken = default)
    {
        GeolocationRequestOptions settings = options ?? DefaultOptions;

        try
        {
            IJSObjectReference module = await GetModuleAsync(cancellationToken);
            TimeSpan timeout = TimeSpan.FromMilliseconds(
                settings.TotalBudgetMilliseconds + INTEROP_MARGIN_MILLISECONDS);

            using CancellationTokenSource cancellation = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            cancellation.CancelAfter(timeout);

            GeolocationJsReading reading = await module.InvokeAsync<GeolocationJsReading>(
                "readPosition", cancellation.Token, settings);

            return reading is null
                ? GeolocationReading.FromFailure(GeolocationFailureReason.Unknown,
                    "The geolocation module answered nothing.")
                : reading.ToReading();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            return GeolocationReading.FromFailure(GeolocationFailureReason.Timeout,
                "The geolocation module did not answer in time.");
        }
        catch (Exception exception)
        {
            return GeolocationReading.FromFailure(GeolocationFailureReason.Unknown, exception.Message);
        }
    }

    public async ValueTask<GeolocationPermissionState> ReadPermissionStateAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            IJSObjectReference module = await GetModuleAsync(cancellationToken);

            using CancellationTokenSource cancellation = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            cancellation.CancelAfter(TimeSpan.FromMilliseconds(PERMISSION_TIMEOUT_MILLISECONDS));

            string state = await module.InvokeAsync<string>("readPermissionState", cancellation.Token);
            return ToPermissionState(state);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            return GeolocationPermissionState.Unknown;
        }
    }

    public async ValueTask<ILatLong> GetPositionAsync() =>
        (await ReadPositionAsync()).Position;

    public async ValueTask<bool> GetGeoLocationGrantedAsync() =>
        await ReadPermissionStateAsync() == GeolocationPermissionState.Granted;

    static GeolocationPermissionState ToPermissionState(string state) => state switch
    {
        "granted" => GeolocationPermissionState.Granted,
        "denied" => GeolocationPermissionState.Denied,
        "prompt" => GeolocationPermissionState.Prompt,
        _ => GeolocationPermissionState.Unknown
    };

    // Not a Lazy: a Lazy that faulted keeps handing the same failure back, and the import
    // fails whenever the app happens to be offline the first time a page asks for it.
    async ValueTask<IJSObjectReference> GetModuleAsync(CancellationToken cancellationToken)
    {
        if (Module is not null)
            return Module;

        await ModuleLock.WaitAsync(cancellationToken);
        try
        {
            if (Module is null)
            {
                using CancellationTokenSource cancellation = CancellationTokenSource
                    .CreateLinkedTokenSource(cancellationToken);
                cancellation.CancelAfter(TimeSpan.FromMilliseconds(MODULE_LOAD_TIMEOUT_MILLISECONDS));

                Module = await JsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", cancellation.Token, $"./{ContentHelper.ContentPath}/geolocation.js?v=132");
            }
        }
        finally
        {
            ModuleLock.Release();
        }

        return Module;
    }

    // Registering the same instance behind two interfaces means the container tracks it
    // twice and disposes it twice, so this has to survive being called more than once.
    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        IJSObjectReference module = Module;
        Module = null;
        ModuleLock.Dispose();

        if (module is null)
            return;

        try
        {
            await module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
        catch (OperationCanceledException)
        {
        }
    }
}
