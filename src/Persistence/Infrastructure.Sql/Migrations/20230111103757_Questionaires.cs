using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Persistence.Sql.Migrations
{
    public partial class Questionaires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_PredefinedAnswers_Questions_QuestionId",
                table: "PredefinedAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswersAsInt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CronExpression",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "HasPredefinedAnswers",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SchedulerInstanceId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Questions",
                newName: "QuestionaireId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "PredefinedAnswers",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Answers",
                newName: "Text");

            migrationBuilder.AddColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RangeMax",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RangeMin",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "PredefinedAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionaireSessionId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Questionaires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionaireSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionaireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionaireSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionaireSessions_Questionaires_QuestionaireId",
                        column: x => x.QuestionaireId,
                        principalTable: "Questionaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionaireId",
                table: "Questions",
                column: "QuestionaireId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionaireSessions_QuestionaireId",
                table: "QuestionaireSessions",
                column: "QuestionaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_PredefinedAnswers_Questions_QuestionId",
                table: "PredefinedAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questionaires_QuestionaireId",
                table: "Questions",
                column: "QuestionaireId",
                principalTable: "Questionaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PredefinedAnswers_Questions_QuestionId",
                table: "PredefinedAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questionaires_QuestionaireId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionaireSessions");

            migrationBuilder.DropTable(
                name: "Questionaires");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionaireId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RangeMax",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RangeMin",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionaireSessionId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "QuestionaireId",
                table: "Questions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "PredefinedAnswers",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Answers",
                newName: "Content");

            migrationBuilder.AddColumn<bool>(
                name: "AnswersAsInt",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CronExpression",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPredefinedAnswers",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SchedulerInstanceId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "PredefinedAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PredefinedAnswers_Questions_QuestionId",
                table: "PredefinedAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
