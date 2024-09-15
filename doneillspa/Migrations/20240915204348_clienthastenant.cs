using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doneillspa.Migrations
{
    public partial class clienthastenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "Client",
                type: "bigint",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Client_TenantId",
                table: "Client",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Tenant_TenantId",
                table: "Client",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Tenant_TenantId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_TenantId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Client");
        }
    }
}
