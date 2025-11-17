namespace BlazorBasics.Maps.Entities.Models;
public class MapClickEventArgs(string markerId, string address, ILatLong point = null, IAddressDetails details = null)
{
    public string MarkerId => markerId;
    public ILatLong Point => point;
    public string Address => address;
    public IAddressDetails Details => details;
}
