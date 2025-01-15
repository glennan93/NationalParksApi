using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NationalParksApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLatitudeAndLongitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Parks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Parks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Parks");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Parks");
        }
    }
}
