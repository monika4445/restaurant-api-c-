using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreatePlayerRequest
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateOnly Dob { get; set; }

    [Required]
    public AddressDto PrimaryAddress { get; set; } = new();

    [Required]
    public AddressDto AlternateAddress { get; set; } = new();

    [Required]
    public AddressDto OfficeAddress { get; set; } = new();

    [Required, Phone, StringLength(50)]
    public string MobileNumber { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string DriversLicense { get; set; } = string.Empty;

    [Required, StringLength(50)]
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
