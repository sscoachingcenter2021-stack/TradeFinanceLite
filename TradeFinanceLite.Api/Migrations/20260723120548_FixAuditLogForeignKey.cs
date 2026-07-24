using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeFinanceLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditLogForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedById",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_PerformedById",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "PerformedById",
                table: "AuditLogs");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_PerformedByUserId",
                table: "AuditLogs",
                column: "PerformedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedByUserId",
                table: "AuditLogs",
                column: "PerformedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Users_PerformedByUserId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_PerformedByUserId",
                table: "AuditLogs");

            migrationBuilder.AddColumn<int>(
                name: "PerformedById",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_PerformedById",
                table: "AuditLogs",
                column: "PerformedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Users_PerformedById",
                table: "AuditLogs",
                column: "PerformedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
