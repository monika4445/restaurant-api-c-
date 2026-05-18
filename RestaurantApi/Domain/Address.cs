namespace RestaurantApi.Domain;

public class Address
{
    public string StreetNumber { get; set; } = string.Empty;
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Postal { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
