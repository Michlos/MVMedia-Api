using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVMedia.Api.Migrations
{
    /// <inheritdoc />
    public partial class IncludeThumbFileNameinMediaFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbFileName",
                table: "MediaFiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbFileName",
                table: "MediaFiles");
        }
    }
}
