using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourtBookingApp.Data.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BookedSlot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(nullable: false),
                    SlotDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedSlot_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookedSlot_BookingId",
                table: "BookedSlot",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "BookedSlot");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
