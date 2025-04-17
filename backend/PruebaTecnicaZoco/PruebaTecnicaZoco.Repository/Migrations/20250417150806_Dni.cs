using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnicaZoco.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Dni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dni",
                table: "Users");
        }
    }
}
