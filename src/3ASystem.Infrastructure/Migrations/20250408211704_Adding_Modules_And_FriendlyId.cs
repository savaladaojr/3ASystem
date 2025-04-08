using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3ASystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Modules_And_FriendlyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functionalities_Applications_ApplicationId",
                table: "Functionalities");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Functionalities",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Functionalities_ApplicationId",
                table: "Functionalities",
                newName: "IX_Functionalities_ModuleId");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyId",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyId",
                table: "Operations",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyId",
                table: "Functionalities",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPartOfMenu",
                table: "Functionalities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FriendlyId",
                table: "Applications",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FriendlyId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPartOfMenu = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_FriendlyId",
                table: "Operations",
                column: "FriendlyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functionalities_FriendlyId",
                table: "Functionalities",
                column: "FriendlyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functionalities_IsPartOfMenu",
                table: "Functionalities",
                column: "IsPartOfMenu");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_FriendlyId",
                table: "Applications",
                column: "FriendlyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Abbreviation",
                table: "Modules",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ApplicationId",
                table: "Modules",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_FriendlyId",
                table: "Modules",
                column: "FriendlyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_IsActive",
                table: "Modules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_IsPartOfMenu",
                table: "Modules",
                column: "IsPartOfMenu");

            migrationBuilder.AddForeignKey(
                name: "FK_Functionalities_Modules_ModuleId",
                table: "Functionalities",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functionalities_Modules_ModuleId",
                table: "Functionalities");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Operations_FriendlyId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Functionalities_FriendlyId",
                table: "Functionalities");

            migrationBuilder.DropIndex(
                name: "IX_Functionalities_IsPartOfMenu",
                table: "Functionalities");

            migrationBuilder.DropIndex(
                name: "IX_Applications_FriendlyId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "FriendlyId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FriendlyId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "FriendlyId",
                table: "Functionalities");

            migrationBuilder.DropColumn(
                name: "IsPartOfMenu",
                table: "Functionalities");

            migrationBuilder.DropColumn(
                name: "FriendlyId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Functionalities",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Functionalities_ModuleId",
                table: "Functionalities",
                newName: "IX_Functionalities_ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Functionalities_Applications_ApplicationId",
                table: "Functionalities",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
