using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Persistence.Sql.Migrations
{
    public partial class Entries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_UserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "EntryType",
                table: "TimeTrackingEntries");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "TimeTrackingEntries",
                newName: "StartTime");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "TimeTrackingEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "TimeTrackingEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "TimeTrackingEntries");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeTrackingEntries");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TimeTrackingEntries",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<int>(
                name: "EntryType",
                table: "TimeTrackingEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
