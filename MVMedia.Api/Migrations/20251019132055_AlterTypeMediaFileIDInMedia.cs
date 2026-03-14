using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVMedia.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterTypeMediaFileIDInMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MediaFileId",
                table: "Medias",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.NewGuid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Medias");
        }
    }
}
