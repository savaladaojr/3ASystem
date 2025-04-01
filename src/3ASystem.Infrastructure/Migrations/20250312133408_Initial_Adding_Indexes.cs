using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3ASystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Adding_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Applications_Hash",
                table: "Applications",
                column: "Hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_Hash",
                table: "Applications");
        }
    }
}
