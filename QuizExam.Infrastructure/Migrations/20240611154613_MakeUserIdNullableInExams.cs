using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullableInExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd2caa91-a491-4f9f-94b4-8bd7e340c9f7", "AQAAAAIAAYagAAAAECtyhn0gbKWUPeevEvJfcWsyOA/Y/+E1ZcMMtSILur7xx4AWWBvca59EAOkptFLyxg==", "4772e371-ba58-4cdf-9b35-ba8291cbe7bc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c52338d1-62e0-4f38-aaa5-9a9eee058076", "AQAAAAIAAYagAAAAEEXIriYxpibLLn15M2RxY3L4mwQVwXx6bfejJif5GN5+mEHKkb8P0VF1RFevCDOWjA==", "945d7acd-8e3d-4e08-9635-7ab0b9dee433" });
        }
    }
}
