using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doneillspa.Migrations
{
    public partial class projectmadechargeable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "Chargeable",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chargeable",
                table: "Project");

        }
    }
}
