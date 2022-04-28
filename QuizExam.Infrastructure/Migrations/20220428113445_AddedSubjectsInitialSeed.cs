using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    public partial class AddedSubjectsInitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("830f38c7-d1f1-4952-8437-68ea7e9555b9"), true, "История и цивилизация" },
                    { new Guid("cce5f418-8df6-437f-b2aa-b1fee0455c85"), true, "Химия и опазване на околната среда" },
                    { new Guid("e45bcdae-9f76-4761-96ef-eedc22fe0aaa"), true, "Математика" },
                    { new Guid("e71efbaf-c151-4cf6-81a0-c1c4aa7a3548"), true, "География и икономика" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("830f38c7-d1f1-4952-8437-68ea7e9555b9"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("cce5f418-8df6-437f-b2aa-b1fee0455c85"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e45bcdae-9f76-4761-96ef-eedc22fe0aaa"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e71efbaf-c151-4cf6-81a0-c1c4aa7a3548"));
        }
    }
}
