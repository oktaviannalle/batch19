using System;

class pembagian
{
    static void Main()
    {
        Console.Write("Masukkan angka pertama : ");
        int angka1 = int.Parse(Console.ReadLine());

        Console.Write("Masukkan angka pembagi : ");
        int angka2 = int.Parse(Console.ReadLine());

        try
        {   // TRY tempat mencoba kode yg rawan error
            int hasil = angka1 /angka2;
            Console.WriteLine($"hasil pembagian: {hasil}");
        }
        catch (DivideByZeroException ex)
        {   //CATCH: Hanya berjalan ketika error:contohnya di bagi dengan 0
            Console.WriteLine("[ERROR] : Anda tidak boleh membagi dengan angka 0!!!");
            Console.WriteLine($"Detail teknis untuk developer: {ex.Message}");               
        }
        finally
        {
            //FINALLY akan berjalan walaupun error / tidak ada error
            Console.WriteLine("Sesi perhitungan selesai");
        }

    }
}
class Cekdataprofil
{
    static void Main()
    {
        // 1. Membuat variabel integer yang BISA BERNILAI NULL (kosong)
        int? umurPengguna = null; // Pengguna belum mengisi umur

        Console.WriteLine("=== Pengecekan Data Profil ===");

        // 2. Mengecek apakah data ada isinya menggunakan .HasValue
        if (umurPengguna.HasValue)
        {
            // Mengambil data asli aman dilakukan di sini menggunakan .Value
            Console.WriteLine($"Umur Pengguna: {umurPengguna.Value} tahun");
        }
        else
        {
            Console.WriteLine("Umur Pengguna: Belum diisi / Tidak diketahui");
        }

        // 3. Menggunakan Operator ?? (Null-Coalescing) untuk nilai cadangan
        // Jika 'umurPengguna' null, maka otomatis gunakan angka cadangan (misal: 0)
        int umurFix = umurPengguna ?? 0;
        Console.WriteLine($"Umur yang dicatat sistem: {umurFix} tahun");
    }
}