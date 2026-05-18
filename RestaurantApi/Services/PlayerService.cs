using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Exceptions;
using RestaurantApi.Mapping;

namespace RestaurantApi.Services;

public class PlayerService : IPlayerService
{
    private readonly AppDbContext _db;

    public PlayerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PlayerResponse> CreateAsync(CreatePlayerRequest request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (request.Dob > today)
        {
            throw new ValidationException("Date of birth cannot be in the future.");
        }

        var email = request.Email.Trim();
        var passport = request.Passport.Trim();
        var license = request.DriversLicense.Trim();

        var existing = await _db.Players
            .Where(p => p.Email.ToLower() == email.ToLower()
                     || p.Passport.ToLower() == passport.ToLower()
                     || p.DriversLicense.ToLower() == license.ToLower())
            .Select(p => new { p.Email, p.Passport, p.DriversLicense })
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is not null)
        {
            if (string.Equals(existing.Email, email, StringComparison.OrdinalIgnoreCase))
                throw new ConflictException($"Player with email '{email}' already exists.");
            if (string.Equals(existing.Passport, passport, StringComparison.OrdinalIgnoreCase))
                throw new ConflictException($"Player with passport '{passport}' already exists.");
            if (string.Equals(existing.DriversLicense, license, StringComparison.OrdinalIgnoreCase))
                throw new ConflictException($"Player with driver's license '{license}' already exists.");
        }

        var player = request.ToEntity();
        _db.Players.Add(player);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new ConflictException("Player with the same email, passport, or driver's license already exists.");
        }

        return player.ToResponse();
    }

    public async Task<IReadOnlyList<PlayerResponse>> SearchByNameAsync(string? firstName, string? lastName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
        {
            throw new ValidationException("Provide firstName or lastName.");
        }

        var query = _db.Players.AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            var pattern = $"%{firstName.Trim()}%";
            query = query.Where(p => EF.Functions.ILike(p.FirstName, pattern));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            var pattern = $"%{lastName.Trim()}%";
            query = query.Where(p => EF.Functions.ILike(p.LastName, pattern));
        }

        var results = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync(cancellationToken);

        return results.Select(p => p.ToResponse()).ToList();
    }
}
