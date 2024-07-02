using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaletteMaster.Repository.Migrations
{
    /// <inheritdoc />
    public partial class HexadecimalColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blue",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "Green",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "Red",
                table: "Colors");

            migrationBuilder.AddColumn<string>(
                name: "Hexadecimal",
                table: "Colors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hexadecimal",
                table: "Colors");

            migrationBuilder.AddColumn<int>(
                name: "Blue",
                table: "Colors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Green",
                table: "Colors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Red",
                table: "Colors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
