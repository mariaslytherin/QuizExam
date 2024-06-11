using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionBetweenExamAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c52338d1-62e0-4f38-aaa5-9a9eee058076", "AQAAAAIAAYagAAAAEEXIriYxpibLLn15M2RxY3L4mwQVwXx6bfejJif5GN5+mEHKkb8P0VF1RFevCDOWjA==", "945d7acd-8e3d-4e08-9635-7ab0b9dee433" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UserId",
                table: "Exams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_UserId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2e1e5c8-3b6c-45ca-b71d-77a936c50861", "AQAAAAIAAYagAAAAEIFOR/jlwFgXvePcMj0kZDA/Xw9ISDgs/whyYDg3ce5518PVJNdrlNR/FeB+d6XStw==", "79cc0332-5229-4341-80de-127aa15d9a22" });
        }
    }
}
