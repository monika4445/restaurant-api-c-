using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Dtos;

public class CreateMembershipRequest
{
    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public Guid RestaurantId { get; set; }
}

public class CreateFavoriteRequest
{
    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public Guid RestaurantId { get; set; }
}

public class MembershipResponse
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RestaurantId { get; set; }
}

public class FavoriteResponse
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RestaurantId { get; set; }
}
