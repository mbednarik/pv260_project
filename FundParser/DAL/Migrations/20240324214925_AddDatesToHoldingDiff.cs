using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundParser.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDatesToHoldingDiff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NewHoldingDate",
                table: "HoldingDiffs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OldHoldingDate",
                table: "HoldingDiffs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewHoldingDate",
                table: "HoldingDiffs");

            migrationBuilder.DropColumn(
                name: "OldHoldingDate",
                table: "HoldingDiffs");
        }
    }
}
