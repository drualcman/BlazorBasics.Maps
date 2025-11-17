namespace BlazorBasics.Maps.Entities.Interfaces;

public interface IAddressDetails
{
    string StreetNumber { get; set; }
    string Route { get; set; }
    string Neighborhood { get; set; }
    /// <summary>
    /// AdministrativeArea typically refers to the state, province, or region.
    /// </summary>
    string AdministrativeArea { get; set; }
    /// <summary>
    /// Locality typically refers to the city or town name.
    /// </summary>
    string Locality { get; set; }
    string PostalCode { get; set; }
    string Country { get; set; }
}