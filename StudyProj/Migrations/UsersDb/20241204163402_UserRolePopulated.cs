using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyProj.Migrations.UsersDb
{
    /// <inheritdoc />
    public partial class UserRolePopulated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "51bf09e1-d994-434f-8dd2-0e32f2341c10", "09343084-a248-4f95-a48b-9879aa9ecf96" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "51bf09e1-d994-434f-8dd2-0e32f2341c10", "09343084-a248-4f95-a48b-9879aa9ecf96" });
        }
    }
}
