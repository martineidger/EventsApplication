using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNotifStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Events_EventId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_EventId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Notification");

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_EventId",
                table: "Notification",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Events_EventId",
                table: "Notification",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
