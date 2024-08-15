using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortenedLinks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkStatisticChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VisitedAt",
                table: "LinksStatistics",
                newName: "VisitDate");

            migrationBuilder.RenameColumn(
                name: "UserAgent",
                table: "LinksStatistics",
                newName: "Device");

            migrationBuilder.RenameColumn(
                name: "Referer",
                table: "LinksStatistics",
                newName: "Country");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "LinksStatistics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "LinksStatistics");

            migrationBuilder.RenameColumn(
                name: "VisitDate",
                table: "LinksStatistics",
                newName: "VisitedAt");

            migrationBuilder.RenameColumn(
                name: "Device",
                table: "LinksStatistics",
                newName: "UserAgent");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "LinksStatistics",
                newName: "Referer");
        }
    }
}
