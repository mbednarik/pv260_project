using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundParser.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtAndDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Holdings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "HoldingDiffs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Funds",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Companies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Funds",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeletedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "DeletedAt" },
                values: new object[] { new DateTime(2024, 3, 15, 11, 50, 20, 511, DateTimeKind.Local).AddTicks(9890), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "HoldingDiffs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "Holdings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2024, 3, 10, 22, 4, 46, 280, DateTimeKind.Local).AddTicks(5402));
        }
    }
}
