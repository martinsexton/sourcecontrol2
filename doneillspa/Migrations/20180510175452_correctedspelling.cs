using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class correctedspelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "Timesheet",
                newName: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Timesheet",
                newName: "username");
        }
    }
}
