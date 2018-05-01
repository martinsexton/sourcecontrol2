using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class updatedtimesheetentryschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "TimesheetEntry");

            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "TimesheetEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "TimesheetEntry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimesheetEntry");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TimesheetEntry");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TimesheetEntry",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
