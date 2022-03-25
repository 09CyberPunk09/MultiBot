using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Sql.Migrations
{
    public partial class RemovedUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramUserId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramUserId",
                table: "Users",
                type: "bigint",
                nullable: true);
        }
    }
}
