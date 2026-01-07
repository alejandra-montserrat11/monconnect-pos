using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStockMinimo1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "StockMinimo",
                table: "Inventarios",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "Inventarios");
        }
    }
}
