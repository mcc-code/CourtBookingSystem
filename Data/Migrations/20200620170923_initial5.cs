using Microsoft.EntityFrameworkCore.Migrations;

namespace CourtBookingApp.Data.Migrations
{
    public partial class initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookingStatus_BookingStatusId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "BookingStatusId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookingStatus_BookingStatusId",
                table: "Booking",
                column: "BookingStatusId",
                principalTable: "BookingStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookingStatus_BookingStatusId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "BookingStatusId",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookingStatus_BookingStatusId",
                table: "Booking",
                column: "BookingStatusId",
                principalTable: "BookingStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
