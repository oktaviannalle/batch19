namespace PendataanBarang.DTOs
{
    public class BarangCreateDTO
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public int Stok { get; set; }
        public int KategoriId { get; set; }
    }
}