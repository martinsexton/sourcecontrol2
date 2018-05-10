using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class addedusernametotimeheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "Timesheet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "Timesheet");
        }
    }
}
