using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace doneillspa.Migrations
{
    public partial class addedclienttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client",
                table: "Project");

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Project",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Project_ClientId",
            //    table: "Project",
            //    column: "ClientId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Project_Client_ClientId",
            //    table: "Project",
            //    column: "ClientId",
            //    principalTable: "Client",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Project_Client_ClientId",
            //    table: "Project");

            migrationBuilder.DropTable(
                name: "Client");

            //migrationBuilder.DropIndex(
            //    name: "IX_Project_ClientId",
            //    table: "Project");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Project",
                nullable: true);
        }
    }
}
