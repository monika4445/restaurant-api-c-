using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreateRestaurantRequest
{
    [Required, StringLength(200)]
    [DefaultValue("Sample Diner")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(500)]
    [DefaultValue("1 Main St")]
    public string Address { get; set; } = string.Empty;

    [Required, Phone, StringLength(50)]
    [DefaultValue("+15550100")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    [DefaultValue("Mon-Sun 9-21")]
    public string HoursOfOperation { get; set; } = string.Empty;
}

public class RestaurantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string HoursOfOperation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class RestaurantWithMemberCountResponse : RestaurantResponse
{
    public int PlayersAgedAtLeastCount { get; set; }
}
