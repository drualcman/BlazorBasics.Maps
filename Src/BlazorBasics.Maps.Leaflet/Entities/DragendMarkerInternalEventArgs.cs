namespace BlazorBasics.Maps.Leaflet.Entities;
public class DragendMarkerInternalEventArgs : EventArgs
{
    public int MarkerId { get; set; }
    public LatLong Point { get; set; }
}
