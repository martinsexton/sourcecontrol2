using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class codeaddedtorpoject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Project");
        }
    }
}
