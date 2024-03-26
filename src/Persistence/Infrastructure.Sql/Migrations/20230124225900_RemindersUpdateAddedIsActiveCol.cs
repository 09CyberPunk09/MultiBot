using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Sql.Migrations
{
    public partial class RemindersUpdateAddedIsActiveCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Reminders");
        }
    }
}
