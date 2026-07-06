// File: Program.cs
using System;

class Program
{
    static void Main()
    {
        PerangkatService service = new PerangkatService();
        bool jalan = true;

        while (jalan)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEM PENDATAAN BARANG BOOTCAMP ===");
            Console.WriteLine("1. Lihat Semua Perangkat");
            Console.WriteLine("2. Tambah Perangkat Baru");
            Console.WriteLine("3. Update Perangkat");
            Console.WriteLine("4. Hapus Data Perangkat");
            Console.WriteLine("5. Keluar");
            Console.Write("Pilih menu (1-5): ");
            
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    Console.WriteLine("\n--- DAFTAR PERANGKAT BOOTCAMP ---");
                    var daftar = service.AmbilSemuaPerangkat(); 
                    
                    if (daftar.Count == 0) Console.WriteLine("Belum ada data.");
                    else
                    {
                        foreach (var p in daftar)
                        {
                            Console.WriteLine($"[{p.Id}] {p.MerkBarang} | Spek: {p.Spesifikasi} | Status: {p.Status} | Stok: {p.Stok}");
                        }
                    }
                    break;

                case "2":
                    Console.WriteLine("\n--- TAMBAH PERANGKAT ---");
                    Console.Write("Masukkan Merk: ");
                    string merk = Console.ReadLine();
                    Console.Write("Masukkan Spesifikasi: ");
                    string spek = Console.ReadLine();
                    Console.Write("Masukkan Jumlah Stok: ");
                    int.TryParse(Console.ReadLine(), out int stokAwal);
                    service.TambahPerangkat(merk, spek, stokAwal);
                    Console.WriteLine("Data berhasil ditambahkan!");
                    break;

                case "3":
                    Console.WriteLine("\n--- UPDATE PERANGKAT ---");
                    Console.Write("Masukkan ID: ");
                    if (int.TryParse(Console.ReadLine(), out int idUpdate))
                    {
                        Console.Write("Masukkan Nama/Merk Baru: ");
                        string namaBaru = Console.ReadLine();

                        Console.Write("Masukkan Jumlah Stok Baru: ");
                        int.TryParse(Console.ReadLine(), out int stokBaru);

                        Console.Write("Masukkan Status baru: ");
                        string status = Console.ReadLine();
                        
                        bool sukses = service.UpdateNamaDanStokDanStatus(idUpdate, namaBaru,stokBaru, status);
                      if (sukses) 
                     {
                         Console.WriteLine("Nama dan Stok barang berhasil diperbarui di database!");
                     }
                         else 
                     {
                         Console.WriteLine("ID tidak ditemukan.");
                     }
                 }
                     break;

                case "4":
                    Console.WriteLine("\n--- HAPUS PERANGKAT ---");
                    Console.Write("Masukkan ID: ");
                    if (int.TryParse(Console.ReadLine(), out int idDelete))
                    {
                        bool sukses = service.HapusPerangkat(idDelete);
                        if (sukses) Console.WriteLine("Data berhasil dihapus!");
                        else Console.WriteLine("ID tidak ditemukan.");
                    }
                    break;

                case "5":
                    jalan = false;
                    break;

                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }

            if (jalan)
            {
                Console.WriteLine("\nTekan Enter untuk kembali...");
                Console.ReadLine();
            }
        }
    }
}