using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PendataanPerangkat.Migrations
{
    /// <inheritdoc />
    public partial class TambahStokPerangkat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stok",
                table: "Perangkats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stok",
                table: "Perangkats");
        }
    }
}
