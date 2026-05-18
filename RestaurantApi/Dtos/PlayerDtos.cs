using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreatePlayerRequest
{
    [Required, StringLength(100)]
    [DefaultValue("Ada")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [DefaultValue("Lovelace")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [DefaultValue("1990-12-10")]
    public DateOnly Dob { get; set; }

    [Required]
    public AddressDto PrimaryAddress { get; set; } = new();

    [Required]
    public AddressDto AlternateAddress { get; set; } = new();

    [Required]
    public AddressDto OfficeAddress { get; set; } = new();

    [Required, Phone, StringLength(50)]
    [DefaultValue("+15550111111")]
    public string MobileNumber { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    [DefaultValue("ada@example.com")]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [DefaultValue("DL-12345")]
    public string DriversLicense { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [DefaultValue("PA-98765")]
    public string Passport { get; set; } = string.Empty;
}

public class PlayerResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly Dob { get; set; }
    public AddressDto PrimaryAddress { get; set; } = new();
    public AddressDto AlternateAddress { get; set; } = new();
    public AddressDto OfficeAddress { get; set; } = new();
    public string MobileNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DriversLicense { get; set; } = string.Empty;
    public string Passport { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PlayerMembershipsResponse : PlayerResponse
{
    public List<RestaurantInMembershipResponse> Restaurants { get; set; } = new();
}

public class PlayerFavoritesResponse : PlayerResponse
{
    public List<RestaurantInFavoritesResponse> Restaurants { get; set; } = new();
}

public class RestaurantInMembershipResponse : RestaurantResponse
{
    public bool IsFavoriteRestaurant { get; set; }
}

public class RestaurantInFavoritesResponse : RestaurantResponse
{
    public bool Linked { get; set; }
}
