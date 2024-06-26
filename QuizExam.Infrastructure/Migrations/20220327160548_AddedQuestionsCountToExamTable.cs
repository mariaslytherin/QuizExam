﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    public partial class AddedQuestionsCountToExamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxScore",
                table: "Exams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuestionsCount",
                table: "Exams",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionsCount",
                table: "Exams");

            migrationBuilder.AlterColumn<int>(
                name: "MaxScore",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
