using System;
using System.Linq;

using(var context = new InventoryDbcontext())
{
    //Create
    Console.WriteLine("Menambahkan barang baru..");
    var barangBaru = new Barang { NamaBarang = "Laptop Asus",Stok = 10};
    context.Barangs.Add(barangBaru);
    context.SaveChanges();

    //Read
    Console.WriteLine("\nDaftar Barang");
    var daftarBarang = context.Barangs.ToList();
    foreach (var b in daftarBarang)
    {
        Console.WriteLine($"- {b.Id}: {b.NamaBarang} (Stok:{b.Stok})");   
    }
    //Update
    Console.WriteLine("\nMengubah stok barang dengan ID 1...");
    var barangDiubah = context.Barangs.FirstOrDefault(b => b.Id == 1);
    if (barangDiubah != null)
    {
        barangDiubah.Stok = 15;
        context.SaveChanges();
        Console.WriteLine($"Stok {barangDiubah.NamaBarang} berhasil di ubah menjadi {barangDiubah.Stok}.");
    }
}