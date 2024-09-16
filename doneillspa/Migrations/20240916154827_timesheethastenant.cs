using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doneillspa.Migrations
{
    public partial class timesheethastenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "Timesheet",
                type: "bigint",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_TenantId",
                table: "Timesheet",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheet_Tenant_TenantId",
                table: "Timesheet",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheet_Tenant_TenantId",
                table: "Timesheet");

            migrationBuilder.DropIndex(
                name: "IX_Timesheet_TenantId",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Timesheet");
        }
    }
}
