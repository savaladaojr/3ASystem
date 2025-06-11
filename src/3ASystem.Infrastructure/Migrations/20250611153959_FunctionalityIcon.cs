using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3ASystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FunctionalityIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Functionalities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Functionalities");
        }
    }
}
