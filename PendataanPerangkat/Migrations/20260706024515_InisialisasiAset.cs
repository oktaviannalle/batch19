using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PendataanPerangkat.Migrations
{
    /// <inheritdoc />
    public partial class InisialisasiAset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perangkats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MerkBarang = table.Column<string>(type: "TEXT", nullable: false),
                    Spesifikasi = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perangkats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perangkats");
        }
    }
}
