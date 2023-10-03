using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCenter.Migrations
{
    /// <inheritdoc />
    public partial class FilePathChangedToFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Posts",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Games",
                newName: "ImageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Posts",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Games",
                newName: "ImagePath");
        }
    }
}
