using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace RestaurantApi.Data;

public static class DbExceptions
{
    private const string PostgresUniqueViolation = "23505";

    public static bool IsUniqueViolation(this DbUpdateException ex) =>
        ex.InnerException is PostgresException pg && pg.SqlState == PostgresUniqueViolation;
}
