using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentGateway.Migrations
{
    /// <inheritdoc />
    public partial class InitialPaymentGateway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GatewayTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ExternalReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PixQrCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PixCopyPaste = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefundedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatewayTransactions_CreatedAt",
                table: "GatewayTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayTransactions_ExternalReference",
                table: "GatewayTransactions",
                column: "ExternalReference");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayTransactions_Status",
                table: "GatewayTransactions",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatewayTransactions");
        }
    }
}
