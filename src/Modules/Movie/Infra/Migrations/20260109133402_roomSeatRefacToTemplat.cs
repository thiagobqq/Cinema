using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Migrations
{
    /// <inheritdoc />
    public partial class roomSeatRefacToTemplat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomSeats_Rooms_RoomId",
                table: "RoomSeats");

            migrationBuilder.DropIndex(
                name: "IX_RoomSeats_RoomId_RowLabel_SeatNumber",
                table: "RoomSeats");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RoomSeats");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SessionSeats");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SessionSeats",
                type: "rowversion", 
                nullable: false    
            );
            migrationBuilder.CreateIndex(
                name: "IX_RoomSeats_RowLabel_SeatNumber",
                table: "RoomSeats",
                columns: new[] { "RowLabel", "SeatNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomSeats_RowLabel_SeatNumber",
                table: "RoomSeats");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SessionSeats");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SessionSeats",
                type: "rowversion",
                nullable: true 
            );

            migrationBuilder.AddColumn<long>(
                name: "RoomId",
                table: "RoomSeats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RoomSeats_RoomId_RowLabel_SeatNumber",
                table: "RoomSeats",
                columns: new[] { "RoomId", "RowLabel", "SeatNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomSeats_Rooms_RoomId",
                table: "RoomSeats",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}