namespace BlazorBasics.Maps.Google.Models;
#nullable enable
public class GeocodeResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = string.Empty;
    public AddressDetails? Details { get; set; }
}
