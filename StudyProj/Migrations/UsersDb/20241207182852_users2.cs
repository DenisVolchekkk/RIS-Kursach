using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyProj.Migrations.UsersDb
{
    /// <inheritdoc />
    public partial class users2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51bf09e1-d994-434f-8dd2-0e32f2341c10",
                columns: new[] { "Description", "Name", "NormalizedName" },
                values: new object[] { "Роль декана", "Dean", "DEAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "51bf09e1-d994-434f-8dd2-0e32f2341c18", null, "Роль преподавателя", "Teacher", "TEACHER" },
                    { "51bf09e1-d994-434f-8dd2-0e32f2341c19", null, "Роль зам. декана", "Deputy Dean", "DEPUTY DEAN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51bf09e1-d994-434f-8dd2-0e32f2341c18");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51bf09e1-d994-434f-8dd2-0e32f2341c19");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51bf09e1-d994-434f-8dd2-0e32f2341c10",
                columns: new[] { "Description", "Name", "NormalizedName" },
                values: new object[] { "Роль админа для пользователя", "Admin", "ADMIN" });
        }
    }
}
