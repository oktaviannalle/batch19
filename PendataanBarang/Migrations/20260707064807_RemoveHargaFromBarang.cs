using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PendataanBarang.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHargaFromBarang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Harga",
                table: "Barangs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Harga",
                table: "Barangs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
