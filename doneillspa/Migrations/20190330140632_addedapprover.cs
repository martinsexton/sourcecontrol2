using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class addedapprover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "HolidayRequest",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayRequest_ApproverId",
                table: "HolidayRequest",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayRequest_AspNetUsers_ApproverId",
                table: "HolidayRequest",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolidayRequest_AspNetUsers_ApproverId",
                table: "HolidayRequest");

            migrationBuilder.DropIndex(
                name: "IX_HolidayRequest_ApproverId",
                table: "HolidayRequest");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "HolidayRequest");
        }
    }
}
