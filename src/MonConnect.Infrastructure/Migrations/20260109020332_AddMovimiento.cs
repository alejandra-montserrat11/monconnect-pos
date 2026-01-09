using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombre",
                table: "Movimientos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "PasswordHash",
                value: "$2a$11$2P.ZAyuc3LLHwS1TLEzPL.oclN.DG7QQeFqy8EwVqf1OUiRFtPujW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioNombre",
                table: "Movimientos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "PasswordHash",
                value: "$2a$11$KwTG0rSerCP9pjWQoJfFv.gyck8q6.EntdsPxvOnjv6eEg1KdyInO");
        }
    }
}
