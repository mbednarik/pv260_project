using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundParser.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cusip = table.Column<string>(type: "TEXT", nullable: false),
                    Ticker = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holdings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FundId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Shares = table.Column<decimal>(type: "TEXT", nullable: false),
                    MarketValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holdings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holdings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Holdings_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldingDiffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoldingId = table.Column<int>(type: "INTEGER", nullable: false),
                    FundId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Shares = table.Column<decimal>(type: "TEXT", nullable: false),
                    SharesChange = table.Column<decimal>(type: "TEXT", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldingDiffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoldingDiffs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoldingDiffs_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoldingDiffs_Holdings_HoldingId",
                        column: x => x.HoldingId,
                        principalTable: "Holdings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Cusip", "Name", "Ticker" },
                values: new object[] { 1, "CUSIP1", "Company1", "TICK1" });

            migrationBuilder.InsertData(
                table: "Funds",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Fund1" });

            migrationBuilder.InsertData(
                table: "Holdings",
                columns: new[] { "Id", "CompanyId", "Date", "FundId", "MarketValue", "Shares", "Weight" },
                values: new object[] { 1, 1, new DateTime(2024, 3, 10, 22, 4, 46, 280, DateTimeKind.Local).AddTicks(5402), 1, 50000m, 1000m, 0.05m });

            migrationBuilder.CreateIndex(
                name: "IX_HoldingDiffs_CompanyId",
                table: "HoldingDiffs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HoldingDiffs_FundId",
                table: "HoldingDiffs",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_HoldingDiffs_HoldingId",
                table: "HoldingDiffs",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_CompanyId",
                table: "Holdings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_FundId",
                table: "Holdings",
                column: "FundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoldingDiffs");

            migrationBuilder.DropTable(
                name: "Holdings");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Funds");
        }
    }
}
