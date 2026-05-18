using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Domain;
using RestaurantApi.Dtos;
using RestaurantApi.Exceptions;
using RestaurantApi.Mapping;

namespace RestaurantApi.Services;

public class MembershipService : IMembershipService
{
    private readonly AppDbContext _db;

    public MembershipService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MembershipResponse> CreateAsync(CreateMembershipRequest request, CancellationToken cancellationToken)
    {
        var playerExists = await _db.Players
            .AnyAsync(p => p.Id == request.PlayerId, cancellationToken);
        if (!playerExists)
        {
            throw new NotFoundException($"Player '{request.PlayerId}' not found.");
        }

        var restaurantExists = await _db.Restaurants
            .AnyAsync(r => r.Id == request.RestaurantId, cancellationToken);
        if (!restaurantExists)
        {
            throw new NotFoundException($"Restaurant '{request.RestaurantId}' not found.");
        }

        var duplicate = await _db.Memberships
            .AnyAsync(m => m.PlayerId == request.PlayerId && m.RestaurantId == request.RestaurantId, cancellationToken);
        if (duplicate)
        {
            throw new ConflictException("Player is already a member of this restaurant.");
        }

        var membership = new Membership
        {
            PlayerId = request.PlayerId,
            RestaurantId = request.RestaurantId
        };
        _db.Memberships.Add(membership);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new ConflictException("Player is already a member of this restaurant.");
        }

        return new MembershipResponse
        {
            Id = membership.Id,
            PlayerId = membership.PlayerId,
            RestaurantId = membership.RestaurantId
        };
    }

    public async Task<IReadOnlyList<PlayerMembershipsResponse>> GetByPlayerNameAsync(string firstName, string lastName, CancellationToken cancellationToken)
    {
        var fn = firstName.Trim();
        var ln = lastName.Trim();

        var players = await _db.Players
            .Where(p => p.FirstName.ToLower() == fn.ToLower() && p.LastName.ToLower() == ln.ToLower())
            .Include(p => p.Memberships)
                .ThenInclude(m => m.Restaurant)
            .Include(p => p.Favorites)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return players
            .Select(p => p.ToMembershipsResponse(p.Favorites.Select(f => f.RestaurantId).ToHashSet()))
            .ToList();
    }
}
