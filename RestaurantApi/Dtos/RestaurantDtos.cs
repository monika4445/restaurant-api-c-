using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreateRestaurantRequest
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [Required, Phone, StringLength(50)]
    public string ContactNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string HoursOfOperation { get; set; } = string.Empty;
}

public class RestaurantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string HoursOfOperation { get; set; } = string.Empty;
}

public class RestaurantWithMemberCountResponse : RestaurantResponse
{
    public int PlayersOverAgeCount { get; set; }
}
