using System.Collections.Generic;
using PendataanPerangkat.Model; 

namespace PendataanPerangkat
{
    public class Kategori
    {
        public int Id { get; set; }
        public string? NamaKategori { get; set; }
        public ICollection<Perangkat> Perangkats { get; set; } = new List<Perangkat>();
    }
}