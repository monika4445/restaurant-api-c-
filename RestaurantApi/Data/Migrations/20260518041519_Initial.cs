using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: false),
                    PrimaryAddress_StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PrimaryAddress_Line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PrimaryAddress_Line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PrimaryAddress_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrimaryAddress_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrimaryAddress_Postal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PrimaryAddress_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AlternateAddress_StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AlternateAddress_Line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AlternateAddress_Line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AlternateAddress_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AlternateAddress_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AlternateAddress_Postal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AlternateAddress_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OfficeAddress_StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OfficeAddress_Line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OfficeAddress_Line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    OfficeAddress_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OfficeAddress_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OfficeAddress_Postal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OfficeAddress_Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DriversLicense = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Passport = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContactNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HoursOfOperation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PlayerId_RestaurantId",
                table: "Favorites",
                columns: new[] { "PlayerId", "RestaurantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_RestaurantId",
                table: "Favorites",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PlayerId_RestaurantId",
                table: "Memberships",
                columns: new[] { "PlayerId", "RestaurantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_RestaurantId",
                table: "Memberships",
                column: "RestaurantId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Name_Address",
                table: "Restaurants",
                columns: new[] { "Name", "Address" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
