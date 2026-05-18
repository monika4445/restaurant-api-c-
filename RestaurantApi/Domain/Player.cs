namespace RestaurantApi.Domain;

public class Player : IAuditable
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly Dob { get; set; }

    public Address PrimaryAddress { get; set; } = new();
    public Address AlternateAddress { get; set; } = new();
    public Address OfficeAddress { get; set; } = new();

    public string MobileNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DriversLicense { get; set; } = string.Empty;
    public string Passport { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
