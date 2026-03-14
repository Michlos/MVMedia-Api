using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVMedia.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMediaFleIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MediaId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Medias");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaId",
                table: "MediaFiles",
                column: "MediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MediaId",
                table: "MediaFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaFileId",
                table: "Medias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaId",
                table: "MediaFiles",
                column: "MediaId",
                unique: true);
        }
    }
}
