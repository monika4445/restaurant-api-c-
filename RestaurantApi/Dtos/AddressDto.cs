using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class AddressDto
{
    [Required, StringLength(50)]
    [DefaultValue("12")]
    public string StreetNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [DefaultValue("Sample St")]
    public string Line1 { get; set; } = string.Empty;

    [StringLength(200)]
    [DefaultValue("Apt 3B")]
    public string? Line2 { get; set; }

    [Required, StringLength(100)]
    [DefaultValue("Yerevan")]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [DefaultValue("YR")]
    public string State { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [DefaultValue("0010")]
    public string Postal { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [DefaultValue("AM")]
    public string Country { get; set; } = string.Empty;
}
