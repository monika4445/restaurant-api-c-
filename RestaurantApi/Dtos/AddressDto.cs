using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class AddressDto
{
    [Required, StringLength(50)]
    public string StreetNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Line1 { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Line2 { get; set; }

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Postal { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;
}
