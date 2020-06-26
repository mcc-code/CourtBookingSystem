using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourtBookingApp.Data.Migrations
{
    public partial class initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotDateTime",
                table: "BookedSlot");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "BookedSlot",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "BookedSlot");

            migrationBuilder.AddColumn<DateTime>(
                name: "SlotDateTime",
                table: "BookedSlot",
                type: "datetime2",
                nullable: true);
        }
    }
}
