using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVMedia.Api.Migrations
{
    /// <inheritdoc />
    public partial class alterMediaFileAsMediaFileToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Medias_MediaId",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "MediaFiles",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_MediaId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MediaFiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MediaFiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Clients_ClientId",
                table: "MediaFiles",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Clients_ClientId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "MediaFiles",
                newName: "MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_ClientId",
                table: "MediaFiles",
                newName: "IX_MediaFiles_MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Medias_MediaId",
                table: "MediaFiles",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
