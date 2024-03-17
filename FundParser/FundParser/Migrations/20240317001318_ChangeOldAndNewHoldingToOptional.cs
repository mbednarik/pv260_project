using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundParser.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOldAndNewHoldingToOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldingDiffs_Holdings_NewHoldingId",
                table: "HoldingDiffs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoldingDiffs_Holdings_OldHoldingId",
                table: "HoldingDiffs");

            migrationBuilder.AlterColumn<int>(
                name: "OldHoldingId",
                table: "HoldingDiffs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "NewHoldingId",
                table: "HoldingDiffs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingDiffs_Holdings_NewHoldingId",
                table: "HoldingDiffs",
                column: "NewHoldingId",
                principalTable: "Holdings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingDiffs_Holdings_OldHoldingId",
                table: "HoldingDiffs",
                column: "OldHoldingId",
                principalTable: "Holdings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldingDiffs_Holdings_NewHoldingId",
                table: "HoldingDiffs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoldingDiffs_Holdings_OldHoldingId",
                table: "HoldingDiffs");

            migrationBuilder.AlterColumn<int>(
                name: "OldHoldingId",
                table: "HoldingDiffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewHoldingId",
                table: "HoldingDiffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingDiffs_Holdings_NewHoldingId",
                table: "HoldingDiffs",
                column: "NewHoldingId",
                principalTable: "Holdings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingDiffs_Holdings_OldHoldingId",
                table: "HoldingDiffs",
                column: "OldHoldingId",
                principalTable: "Holdings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
