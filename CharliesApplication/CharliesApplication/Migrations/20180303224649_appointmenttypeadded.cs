using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CharliesApplication.Migrations
{
    public partial class appointmenttypeadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointment",
                newName: "DueDate");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Appointment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Appointment",
                newName: "Date");
        }
    }
}
