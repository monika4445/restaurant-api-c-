using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseInsensitiveUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_Name_Address",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Players_DriversLicense",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_Email",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_Passport",
                table: "Players");

            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Restaurants_Name_Address_CI"" ON ""Restaurants"" (LOWER(""Name""), LOWER(""Address""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Players_Email_CI"" ON ""Players"" (LOWER(""Email""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Players_Passport_CI"" ON ""Players"" (LOWER(""Passport""));");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IX_Players_DriversLicense_CI"" ON ""Players"" (LOWER(""DriversLicense""));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Restaurants_Name_Address_CI"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Players_Email_CI"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Players_Passport_CI"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_Players_DriversLicense_CI"";");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Name_Address",
                table: "Restaurants",
                columns: new[] { "Name", "Address" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_DriversLicense",
                table: "Players",
                column: "DriversLicense",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Passport",
                table: "Players",
                column: "Passport",
                unique: true);
        }
    }
}
