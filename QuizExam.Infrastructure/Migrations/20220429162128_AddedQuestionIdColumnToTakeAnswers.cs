using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    public partial class AddedQuestionIdColumnToTakeAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "TakeAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TakeAnswers_QuestionId",
                table: "TakeAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakeAnswers_Questions_QuestionId",
                table: "TakeAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakeAnswers_Questions_QuestionId",
                table: "TakeAnswers");

            migrationBuilder.DropIndex(
                name: "IX_TakeAnswers_QuestionId",
                table: "TakeAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TakeAnswers");
        }
    }
}
