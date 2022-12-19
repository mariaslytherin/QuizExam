using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizExam.Infrastructure.Data.Migrations
{
    public partial class UsersSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "daab2d3d-6652-4ef9-9d43-0ec02c7dc78f", "daab2d3d-6652-4ef9-9d43-0ec02c7dc78f", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "02174cf0–9412–4cfe-afbf-59f706d72cf6", 0, "de8a100b-eb66-4c5e-8558-4f7ff3430af1", "admin@admin.com", true, "Admin", "Admin", false, null, null, "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEB7aLArCMa/GIh3gOyA/vb4sFPPjLiQjMEKvIB/F8QCtdt7t6HsXV4tPMe/jkDMC3g==", null, false, "56ffd34e-d3dc-421e-9dad-922ae53a031c", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "daab2d3d-6652-4ef9-9d43-0ec02c7dc78f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6");
        }
    }
}
