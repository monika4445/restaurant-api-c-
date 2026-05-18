namespace RestaurantApi.Domain;

public class Membership
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RestaurantId { get; set; }

    public Player Player { get; set; } = null!;
    public Restaurant Restaurant { get; set; } = null!;
}
