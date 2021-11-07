using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Sql.Migrations
{
    public partial class SetData_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<short>(type: "smallint", nullable: false),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListNotes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListNotes");
        }
    }
}
