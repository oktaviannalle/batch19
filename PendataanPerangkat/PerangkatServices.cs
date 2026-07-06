// File: PerangkatService.cs
using System.Collections.Generic;
using System.Linq;

public class PerangkatService
{
    // READ
    public List<Perangkat> AmbilSemuaPerangkat()
    {
        using (var context = new AppDbContext())
        {
            return context.Perangkats.ToList();
        }
    }

    // CREATE
    public void TambahPerangkat(string merk, string spesifikasi, int stok)
    {
        using (var context = new AppDbContext())
        {
            var perangkatBaru = new Perangkat
            { 
                MerkBarang = merk, 
                Spesifikasi = spesifikasi, 
                Status = "Tersedia",
                Stok = stok
            };
            
            context.Perangkats.Add(perangkatBaru);
            context.SaveChanges();
        }
    }

    // UPDATE

   public bool UpdateNamaDanStokDanStatus(int id, string namaBaru, int stokBaru, string statusBaru)
{
    using (var context = new AppDbContext())
    {
        var data = context.Perangkats.FirstOrDefault(p => p.Id == id);
        if (data != null)
        {
            data.MerkBarang = namaBaru;
            data.Stok = stokBaru;
            data.Status = statusBaru;

            context.SaveChanges();
            return true;
        }
        return false;
    }
}
    public bool UpdateStatus(int id, string statusBaru)
    {
        using (var context = new AppDbContext())
        {
            var data = context.Perangkats.FirstOrDefault(p => p.Id == id);
            if (data != null)
            {
                data.Status = statusBaru;
                context.SaveChanges();
                return true; 
            }
            return false; 
        }
    }

    // DELETE
    public bool HapusPerangkat(int id)
    {
        using (var context = new AppDbContext())
        {
            var data = context.Perangkats.FirstOrDefault(p => p.Id == id);
            if (data != null)
            {
                context.Perangkats.Remove(data);
                context.SaveChanges();
                return true; 
            }
            return false; 
        }
    }
}