using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DerpeWeather.Migrations
{
    /// <inheritdoc />
    public partial class ResolvedLocation_added_to_UserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolvedLocation",
                table: "UserTrackedWeatherFields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolvedLocation",
                table: "UserTrackedWeatherFields");
        }
    }
}
