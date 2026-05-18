using Microsoft.EntityFrameworkCore;
using Npgsql;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Exceptions;
using RestaurantApi.Mapping;

namespace RestaurantApi.Services;

public class PlayerService : IPlayerService
{
    private const string PostgresUniqueViolation = "23505";
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
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == PostgresUniqueViolation)
        {
            throw new ConflictException("Player with the same email, passport, or driver's license already exists.");
        }

        return player.ToResponse();
    }
}
