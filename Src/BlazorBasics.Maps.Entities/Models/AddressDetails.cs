namespace BlazorBasics.Maps.Entities.Models;
public class AddressDetails : IAddressDetails
{
    public string StreetNumber { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    /// <summary>
    /// Locality typically refers to the city or town name.
    /// </summary>
    public string Locality { get; set; } = string.Empty;
    /// <summary>
    /// AdministrativeArea typically refers to the state, province, or region.
    /// </summary>
    public string AdministrativeArea { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
