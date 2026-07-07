namespace PendataanBarang.DTOs
{
    public class BarangDTO
    {
        public int Id { get; set; }
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public int Stok { get; set; }
        public string NamaKategori { get; set; } = string.Empty; // dari relasi
    }
}