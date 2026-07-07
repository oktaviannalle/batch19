// File: PerangkatServices.cs
using System;
using System.Collections.Generic;
using System.Linq;
using PendataanPerangkat.Model; // Untuk mengenali model Perangkat dan Kategori

namespace PendataanPerangkat
{
    public class PerangkatService
    {
        // Fungsi 1: Ambil Semua Data
        public List<Perangkat> AmbilSemuaPerangkat()
        {
            using (var context = new AppDbContext())
            {
                return context.Perangkats.ToList();
            }
        }

        // Fungsi 2: Tambah Data (Sesuai panggilan di Program.cs)
        public void TambahPerangkat(string merk, string spesifikasi, int stok)
        {
            using (var context = new AppDbContext())
            {
                var perangkatBaru = new Perangkat 
                { 
                    MerkBarang = merk, 
                    Spesifikasi = spesifikasi, 
                    Stok = stok,
                    Status = "Tersedia" // Status default
                };
                
                context.Perangkats.Add(perangkatBaru);
                context.SaveChanges();
            }
        }

        // Fungsi 3: Update Data (Sesuai panggilan di Program.cs)
        public bool UpdateNamaDanStokDanStatus(int id, string namaBaru, int stokBaru, string statusBaru)
        {
            using (var context = new AppDbContext())
            {
                var perangkat = context.Perangkats.Find(id);
                if (perangkat != null)
                {
                    perangkat.MerkBarang = namaBaru;
                    perangkat.Stok = stokBaru;
                    perangkat.Status = statusBaru;
                    
                    context.SaveChanges();
                    return true; // Berhasil
                }
                return false; // Gagal, ID tidak ditemukan
            }
        }

        // Fungsi 4: Hapus Data (Sesuai panggilan di Program.cs)
        public bool HapusPerangkat(int id)
        {
            using (var context = new AppDbContext())
            {
                var perangkat = context.Perangkats.Find(id);
                if (perangkat != null)
                {
                    context.Perangkats.Remove(perangkat);
                    context.SaveChanges();
                    return true; // Berhasil
                }
                return false; // Gagal, ID tidak ditemukan
            }
        }
    }
}