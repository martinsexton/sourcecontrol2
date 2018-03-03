using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CharliesApplication.Migrations
{
    public partial class newproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baby",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baby", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirthDetails",
                columns: table => new
                {
                    BabyId = table.Column<long>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Hospital = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthDetails", x => x.BabyId);
                    table.ForeignKey(
                        name: "FK_BirthDetails_Baby_BabyId",
                        column: x => x.BabyId,
                        principalTable: "Baby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthDetails");

            migrationBuilder.DropTable(
                name: "Baby");
        }
    }
}
