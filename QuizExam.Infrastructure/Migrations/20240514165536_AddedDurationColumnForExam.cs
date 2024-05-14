using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDurationColumnForExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Exams",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6cd17b1-bac8-4fbc-83c0-b36a71338bbd", "AQAAAAIAAYagAAAAEP+CqDOsFVyQDCQMGxeX+mNG15vGDvB9qMOEmKRKXMxdqKV4MtfiCJuZJzauKS0POA==", "9ae282e8-da26-4670-acd0-63c9d5db9b5a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01090239-8e41-408c-adb6-82f34202b178", "AQAAAAEAACcQAAAAEN3sfNGfQ9Iof9vwaO6Ud9zWmLZu+73UxQ/jVE8cCzVPeHrM/IUWbxXVQYazQzhxbA==", "472558fb-a318-461d-8528-4dec73287bea" });
        }
    }
}
