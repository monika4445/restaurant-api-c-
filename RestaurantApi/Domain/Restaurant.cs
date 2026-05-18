namespace RestaurantApi.Domain;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string HoursOfOperation { get; set; } = string.Empty;

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
