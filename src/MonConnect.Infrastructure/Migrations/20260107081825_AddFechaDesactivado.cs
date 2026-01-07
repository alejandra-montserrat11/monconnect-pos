using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaDesactivado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDesactivado",
                table: "Sucursales",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivo",
                table: "Sucursales",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDesactivado",
                table: "Productos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivo",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDesactivado",
                table: "Sucursales");

            migrationBuilder.DropColumn(
                name: "IsActivo",
                table: "Sucursales");

            migrationBuilder.DropColumn(
                name: "FechaDesactivado",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "IsActivo",
                table: "Productos");
        }
    }
}
