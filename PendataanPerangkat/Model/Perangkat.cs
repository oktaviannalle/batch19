using PendataanPerangkat;

namespace PendataanPerangkat.Model
{
    public class Perangkat
    {
        public int Id { get; set; }
        public string? MerkBarang { get; set; }
        public string? Spesifikasi { get; set; }
        public string? Status { get; set; }
        public int Stok { get; set; }

        public int KategoriId { get; set; }
        public Kategori? Kategori { get; set; }
    }
}