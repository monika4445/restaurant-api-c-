using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Domain;
using RestaurantApi.Dtos;
using RestaurantApi.Exceptions;
using RestaurantApi.Mapping;

namespace RestaurantApi.Services;

public class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _db;

    public FavoriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<FavoriteResponse> CreateAsync(CreateFavoriteRequest request, CancellationToken cancellationToken)
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

        var duplicate = await _db.Favorites
            .AnyAsync(f => f.PlayerId == request.PlayerId && f.RestaurantId == request.RestaurantId, cancellationToken);
        if (duplicate)
        {
            throw new ConflictException("Restaurant is already a favorite of this player.");
        }

        var favorite = new Favorite
        {
            PlayerId = request.PlayerId,
            RestaurantId = request.RestaurantId
        };
        _db.Favorites.Add(favorite);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new ConflictException("Restaurant is already a favorite of this player.");
        }

        return new FavoriteResponse
        {
            Id = favorite.Id,
            PlayerId = favorite.PlayerId,
            RestaurantId = favorite.RestaurantId
        };
    }

    public async Task<IReadOnlyList<PlayerFavoritesResponse>> GetByPlayerNameAsync(string firstName, string lastName, CancellationToken cancellationToken)
    {
        var fn = firstName.Trim();
        var ln = lastName.Trim();

        var players = await _db.Players
            .Where(p => p.FirstName.ToLower() == fn.ToLower() && p.LastName.ToLower() == ln.ToLower())
            .Include(p => p.Favorites)
                .ThenInclude(f => f.Restaurant)
            .Include(p => p.Memberships)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return players
            .Select(p => p.ToFavoritesResponse(p.Memberships.Select(m => m.RestaurantId).ToHashSet()))
            .ToList();
    }
}
