namespace PendataanBarang.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string NamaKategori { get; set; } = string.Empty;
        public ICollection<Barang> Barangs { get; set; } = new List<Barang>();
    }
}