using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingRequestUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookingRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_UserId",
                table: "BookingRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRequests_AspNetUsers_UserId",
                table: "BookingRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRequests_AspNetUsers_UserId",
                table: "BookingRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookingRequests_UserId",
                table: "BookingRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookingRequests");
        }
    }
}
