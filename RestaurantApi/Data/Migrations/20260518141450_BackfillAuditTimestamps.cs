using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class BackfillAuditTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Restaurants"" SET ""CreatedAt"" = NOW() WHERE ""CreatedAt"" = '-infinity';");
            migrationBuilder.Sql(@"UPDATE ""Players""     SET ""CreatedAt"" = NOW() WHERE ""CreatedAt"" = '-infinity';");
            migrationBuilder.Sql(@"UPDATE ""Memberships"" SET ""CreatedAt"" = NOW() WHERE ""CreatedAt"" = '-infinity';");
            migrationBuilder.Sql(@"UPDATE ""Favorites""   SET ""CreatedAt"" = NOW() WHERE ""CreatedAt"" = '-infinity';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
