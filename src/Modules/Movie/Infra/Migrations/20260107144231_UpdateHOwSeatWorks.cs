using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHOwSeatWorks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Films_FilmId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Rooms_RoomId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_SessionSeats_SessionId_RowLabel_SeatNumber",
                table: "SessionSeats");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_RoomId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RowLabel",
                table: "SessionSeats");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "SessionSeats");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Rooms");

            migrationBuilder.AddColumn<long>(
                name: "RoomSeatId",
                table: "SessionSeats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SessionSeats",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketCode",
                table: "SessionSeats",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Rooms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Films",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "RoomSeats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    RowLabel = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomSeats_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionSeats_RoomSeatId",
                table: "SessionSeats",
                column: "RoomSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionSeats_SessionId_RoomSeatId",
                table: "SessionSeats",
                columns: new[] { "SessionId", "RoomSeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RoomId_StartsAt",
                table: "Sessions",
                columns: new[] { "RoomId", "StartsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StartsAt",
                table: "Sessions",
                column: "StartsAt");

            migrationBuilder.CreateIndex(
                name: "IX_Films_Title",
                table: "Films",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_RoomSeats_RoomId_RowLabel_SeatNumber",
                table: "RoomSeats",
                columns: new[] { "RoomId", "RowLabel", "SeatNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Films_FilmId",
                table: "Sessions",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Rooms_RoomId",
                table: "Sessions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionSeats_RoomSeats_RoomSeatId",
                table: "SessionSeats",
                column: "RoomSeatId",
                principalTable: "RoomSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Films_FilmId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Rooms_RoomId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionSeats_RoomSeats_RoomSeatId",
                table: "SessionSeats");

            migrationBuilder.DropTable(
                name: "RoomSeats");

            migrationBuilder.DropIndex(
                name: "IX_SessionSeats_RoomSeatId",
                table: "SessionSeats");

            migrationBuilder.DropIndex(
                name: "IX_SessionSeats_SessionId_RoomSeatId",
                table: "SessionSeats");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_RoomId_StartsAt",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_StartsAt",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Films_Title",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "RoomSeatId",
                table: "SessionSeats");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SessionSeats");

            migrationBuilder.DropColumn(
                name: "TicketCode",
                table: "SessionSeats");

            migrationBuilder.AddColumn<string>(
                name: "RowLabel",
                table: "SessionSeats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeatNumber",
                table: "SessionSeats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Films",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_SessionSeats_SessionId_RowLabel_SeatNumber",
                table: "SessionSeats",
                columns: new[] { "SessionId", "RowLabel", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RoomId",
                table: "Sessions",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Films_FilmId",
                table: "Sessions",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Rooms_RoomId",
                table: "Sessions",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
