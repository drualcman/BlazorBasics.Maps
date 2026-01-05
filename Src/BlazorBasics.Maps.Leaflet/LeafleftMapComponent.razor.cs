namespace BlazorBasics.Maps.Leaflet;

public partial class LeafleftMapComponent : IAsyncDisposable
{
    protected override void OnInitialized()
    {
        ObjRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await CreateMap(OriginalPoint, ZoomLevel);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await LeafletService.InvokeVoidAsync("deleteMap", MapId);
        ObjRef?.Dispose();
    }
}