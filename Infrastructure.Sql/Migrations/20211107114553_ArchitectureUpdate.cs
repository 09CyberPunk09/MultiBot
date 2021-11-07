using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Sql.Migrations
{
    public partial class ArchitectureUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListNotes");

            migrationBuilder.RenameColumn(
                name: "LastModification",
                table: "Sets",
                newName: "ModificationTime");

            migrationBuilder.RenameColumn(
                name: "LastModification",
                table: "Notes",
                newName: "ModificationTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Sets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SetDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<short>(type: "smallint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetDatas_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetDatas_SetId",
                table: "SetDatas",
                column: "SetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetDatas");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "ModificationTime",
                table: "Sets",
                newName: "LastModification");

            migrationBuilder.RenameColumn(
                name: "ModificationTime",
                table: "Notes",
                newName: "LastModification");

            migrationBuilder.CreateTable(
                name: "ListNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<short>(type: "smallint", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListNotes", x => x.Id);
                });
        }
    }
}
