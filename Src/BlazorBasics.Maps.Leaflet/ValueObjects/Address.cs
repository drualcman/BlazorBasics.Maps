namespace BlazorBasics.Maps.Leaflet.ValueObjects;
public record struct Address(string Street, string City, string State, string PostalCode, string Country) : IAddressDetails
{
    public string StreetNumber
    {
        get => Street;
        set => Street = value;
    }
    public string Route
    {
        get => string.Empty;
        set => Console.WriteLine(value);
    }
    public string Neighborhood
    {
        get => string.Empty;
        set => Console.WriteLine(value);
    }
    public string Locality
    {
        get => City;
        set => City = value;
    }
    public string AdministrativeArea
    {
        get => State;
        set => State = value;
    }
}
