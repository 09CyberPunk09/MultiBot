using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Sql.Migrations
{
    public partial class RemindersUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recuring",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Reminders");

            migrationBuilder.RenameColumn(
                name: "RecuringCron",
                table: "Reminders",
                newName: "SchedulerExpression");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchedulerExpression",
                table: "Reminders",
                newName: "RecuringCron");

            migrationBuilder.AddColumn<bool>(
                name: "Recuring",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderTime",
                table: "Reminders",
                type: "datetime2",
                nullable: true);
        }
    }
}
