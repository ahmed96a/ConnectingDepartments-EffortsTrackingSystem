using Microsoft.EntityFrameworkCore.Migrations;

namespace EffortTrackingSystem.API.Migrations
{
    public partial class ModifyNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReceiverEmail",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Notifications",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverEmail",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
