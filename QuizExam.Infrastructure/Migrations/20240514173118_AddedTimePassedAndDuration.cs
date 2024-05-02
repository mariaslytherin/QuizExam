using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimePassedAndDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "TakeExams",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimePassed",
                table: "TakeExams",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "48a57252-3637-4fc6-9d9c-e7fd71ded59b", "AQAAAAIAAYagAAAAEDlSROLv3o8d1/pdFpuOGPGOWRkae95S87RJFporralac1u579+ljuebuVbD0CacGw==", "17f27cda-9eca-4a80-845e-2166b8906e58" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TakeExams");

            migrationBuilder.DropColumn(
                name: "TimePassed",
                table: "TakeExams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6cd17b1-bac8-4fbc-83c0-b36a71338bbd", "AQAAAAIAAYagAAAAEP+CqDOsFVyQDCQMGxeX+mNG15vGDvB9qMOEmKRKXMxdqKV4MtfiCJuZJzauKS0POA==", "9ae282e8-da26-4670-acd0-63c9d5db9b5a" });
        }
    }
}
