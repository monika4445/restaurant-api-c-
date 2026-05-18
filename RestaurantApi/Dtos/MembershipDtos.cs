using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreateMembershipRequest
{
    [Required]
    [Description("Existing player Id. Create one via POST /api/players first, then paste the returned Id here.")]
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    public Guid PlayerId { get; set; }

    [Required]
    [Description("Existing restaurant Id. Create one via POST /api/restaurants first, then paste the returned Id here.")]
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    public Guid RestaurantId { get; set; }
}

public class CreateFavoriteRequest
{
    [Required]
    [Description("Existing player Id. Create one via POST /api/players first, then paste the returned Id here.")]
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    public Guid PlayerId { get; set; }

    [Required]
    [Description("Existing restaurant Id. Create one via POST /api/restaurants first, then paste the returned Id here.")]
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    public Guid RestaurantId { get; set; }
}

public class MembershipResponse
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FavoriteResponse
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
