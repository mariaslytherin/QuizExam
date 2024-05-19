using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTakeExamModeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "TakeExams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9f4c7292-ea4c-4963-b254-d5022e209c2a", "AQAAAAIAAYagAAAAEG0ssUBASDpZoQcNdcxIOMJ2vzYahZxoxjcREs8bc4O0BPj9/KYi2T5pyKagW5Fy2g==", "5d958abc-5b28-4f7d-92dc-0495b74fe1fa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "TakeExams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "48a57252-3637-4fc6-9d9c-e7fd71ded59b", "AQAAAAIAAYagAAAAEDlSROLv3o8d1/pdFpuOGPGOWRkae95S87RJFporralac1u579+ljuebuVbD0CacGw==", "17f27cda-9eca-4a80-845e-2166b8906e58" });
        }
    }
}
