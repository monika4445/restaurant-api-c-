using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Exceptions;
using RestaurantApi.Mapping;

namespace RestaurantApi.Services;

public class RestaurantService : IRestaurantService
{
    private readonly AppDbContext _db;

    public RestaurantService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var address = request.Address.Trim();

        var duplicate = await _db.Restaurants
            .AnyAsync(r => r.Name.ToLower() == name.ToLower()
                        && r.Address.ToLower() == address.ToLower(),
                cancellationToken);

        if (duplicate)
        {
            throw new ConflictException($"Restaurant '{name}' at '{address}' already exists.");
        }

        var restaurant = request.ToEntity();
        _db.Restaurants.Add(restaurant);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new ConflictException($"Restaurant '{name}' at '{address}' already exists.");
        }

        return restaurant.ToResponse();
    }

    public async Task<IReadOnlyList<RestaurantResponse>> SearchByNameAsync(string name, CancellationToken cancellationToken)
    {
        var pattern = $"%{name.Trim()}%";
        var results = await _db.Restaurants
            .Where(r => EF.Functions.ILike(r.Name, pattern))
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        return results.Select(r => r.ToResponse()).ToList();
    }
}
