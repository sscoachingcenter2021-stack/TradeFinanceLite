using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeFinanceLite.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRemarksToLC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LettersOfCredit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LettersOfCredit");
        }
    }
}
