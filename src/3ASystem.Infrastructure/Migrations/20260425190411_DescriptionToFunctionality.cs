using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3ASystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionToFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Functionalities",
                type: "ntext",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Functionalities");
        }
    }
}
