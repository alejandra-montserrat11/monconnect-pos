using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuariosAndSeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "IsActivo", "PasswordHash", "Rol" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), "admin@monconnect.com", true, "$2a$11$W/XbNQlTTqQRVqRHDpiCnepy82iJB4KzfcYL6iNve7vo/9UcDmCmO", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));
        }
    }
}
