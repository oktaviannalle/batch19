using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PendataanPerangkat.Migrations
{
    /// <inheritdoc />
    public partial class TambahTabelKategori : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Perangkats",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Spesifikasi",
                table: "Perangkats",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MerkBarang",
                table: "Perangkats",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Perangkats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Kategoris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamaKategori = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoris", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Perangkats_KategoriId",
                table: "Perangkats",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Perangkats_Kategoris_KategoriId",
                table: "Perangkats",
                column: "KategoriId",
                principalTable: "Kategoris",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perangkats_Kategoris_KategoriId",
                table: "Perangkats");

            migrationBuilder.DropTable(
                name: "Kategoris");

            migrationBuilder.DropIndex(
                name: "IX_Perangkats_KategoriId",
                table: "Perangkats");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Perangkats");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Perangkats",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Spesifikasi",
                table: "Perangkats",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MerkBarang",
                table: "Perangkats",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
