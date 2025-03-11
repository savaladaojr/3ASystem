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
            migrationBuilder.DropIndex(
                name: "IX_Roles_Code",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Functionalities_Abbreviation",
                table: "Functionalities");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Abbreviation",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                table: "Roles",
                column: "Code",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Functionalities_Abbreviation",
                table: "Functionalities",
                column: "Abbreviation",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Abbreviation",
                table: "Applications",
                column: "Abbreviation",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Hash",
                table: "Applications",
                column: "Hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Code",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Functionalities_Abbreviation",
                table: "Functionalities");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Abbreviation",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Hash",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functionalities_Abbreviation",
                table: "Functionalities",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Abbreviation",
                table: "Applications",
                column: "Abbreviation",
                unique: true);
        }
    }
}
