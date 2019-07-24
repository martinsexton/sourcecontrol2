using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class renamedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Project_Client_ClientId",
            //    table: "Project");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Project",
                newName: "OwningClientId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Project_ClientId",
            //    table: "Project",
            //    newName: "IX_Project_OwningClientId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Project_Client_OwningClientId",
            //    table: "Project",
            //    column: "OwningClientId",
            //    principalTable: "Client",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Project_Client_OwningClientId",
            //    table: "Project");

            migrationBuilder.RenameColumn(
                name: "OwningClientId",
                table: "Project",
                newName: "ClientId");

            //migrationBuilder.RenameIndex(
                //name: "IX_Project_OwningClientId",
                //table: "Project",
                //newName: "IX_Project_ClientId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Project_Client_ClientId",
            //    table: "Project",
            //    column: "ClientId",
            //    principalTable: "Client",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
