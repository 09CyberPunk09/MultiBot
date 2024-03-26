using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Sql.Migrations
{
    public partial class TelegramLogins_ChatIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "TelegramLogins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "TelegramLogins");
        }
    }
}
