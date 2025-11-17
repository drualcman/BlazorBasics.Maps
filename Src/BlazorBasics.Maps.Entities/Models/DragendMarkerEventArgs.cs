namespace BlazorBasics.Maps.Entities.Models;
public class DragendMarkerEventArgs : EventArgs
{
    public int MarkerId { get; set; }
    public ILatLong Point { get; set; }
}
