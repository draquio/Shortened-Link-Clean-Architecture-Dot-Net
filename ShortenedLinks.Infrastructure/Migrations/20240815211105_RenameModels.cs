using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortenedLinks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortenedUrl",
                table: "Links",
                newName: "ShortenedLink");

            migrationBuilder.RenameColumn(
                name: "OriginalUrl",
                table: "Links",
                newName: "OriginalLink");

            migrationBuilder.RenameIndex(
                name: "IX_Links_ShortenedUrl",
                table: "Links",
                newName: "IX_Links_ShortenedLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortenedLink",
                table: "Links",
                newName: "ShortenedUrl");

            migrationBuilder.RenameColumn(
                name: "OriginalLink",
                table: "Links",
                newName: "OriginalUrl");

            migrationBuilder.RenameIndex(
                name: "IX_Links_ShortenedLink",
                table: "Links",
                newName: "IX_Links_ShortenedUrl");
        }
    }
}
