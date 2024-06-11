using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamsDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8ef1f00-2a32-4c15-bc89-76db89cb0df4", "AQAAAAIAAYagAAAAEMJqEBigUpChYpk96Oqe/xAMUyxh/YROOC8mkYxN6/m7fbKx8XYkQEsLpBRXGyQyBQ==", "3a1db7a4-e689-4b5b-84b4-8e1faa3225f1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd2caa91-a491-4f9f-94b4-8bd7e340c9f7", "AQAAAAIAAYagAAAAECtyhn0gbKWUPeevEvJfcWsyOA/Y/+E1ZcMMtSILur7xx4AWWBvca59EAOkptFLyxg==", "4772e371-ba58-4cdf-9b35-ba8291cbe7bc" });

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_UserId",
                table: "Exams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
