using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCenter.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeKeysToGameToPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameToPlatform",
                table: "GameToPlatform");

            migrationBuilder.DropIndex(
                name: "IX_GameToPlatform_GameId",
                table: "GameToPlatform");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameToPlatform");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameToPlatform",
                table: "GameToPlatform",
                columns: new[] { "GameId", "PlatformId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameToPlatform",
                table: "GameToPlatform");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GameToPlatform",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameToPlatform",
                table: "GameToPlatform",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameToPlatform_GameId",
                table: "GameToPlatform",
                column: "GameId");
        }
    }
}