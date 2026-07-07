namespace PendataanBarang.Models
{
    public class Barang
    {
        public int Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public int Stok { get; set; }

        // Foreign Key
        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; }
    }
}