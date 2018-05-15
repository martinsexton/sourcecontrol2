using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class CertificationManyToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certification_AspNetUsers_ApplicationUserId",
                table: "Certification");

            migrationBuilder.DropIndex(
                name: "IX_Certification_ApplicationUserId",
                table: "Certification");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Certification");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Certification",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Certification_UserId",
                table: "Certification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certification_AspNetUsers_UserId",
                table: "Certification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certification_AspNetUsers_UserId",
                table: "Certification");

            migrationBuilder.DropIndex(
                name: "IX_Certification_UserId",
                table: "Certification");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Certification");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Certification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certification_ApplicationUserId",
                table: "Certification",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certification_AspNetUsers_ApplicationUserId",
                table: "Certification",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
