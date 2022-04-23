using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Sql.Migrations
{
    public partial class JobExecutorInstanceIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchedulerInstanceId",
                table: "Reminders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchedulerInstanceId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchedulerInstanceId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "SchedulerInstanceId",
                table: "Questions");
        }
    }
}
