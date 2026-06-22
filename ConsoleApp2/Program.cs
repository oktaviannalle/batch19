// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
//SYNTAX
{
string nama;
int umurr;
Console.WriteLine("Masukan nama : ");
nama = Console.ReadLine();
Console.WriteLine("Masukkan Umur anda : ");
umurr = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Hallo " + nama);
Console.WriteLine("Umur kamu " + umurr +"tahun");
}
//TypeBasic
int umur = 15;

bool dewasa = umur >= 18;

if (dewasa)
{
    Console.WriteLine("Anda sudah dewasa");
}
else
{
    Console.WriteLine("Anda masih ank anak");
}
//NUMERIC TYPE

Console.Write("Masukkan Panjang : ");
double panjang = Convert.ToDouble(Console.ReadLine());

Console.Write("Masukkan Lebar : ");
double lebar = Convert.ToDouble(Console.ReadLine());

double luas = panjang * lebar ;

Console.WriteLine("Luas " + luas);

//BOOLEAN TYPE AND OPERATORS

Console.WriteLine("Masukkan nilai : ");
int nilai = Convert.ToInt32(Console.ReadLine());

bool lulus = nilai >= 75;

if (lulus)
{
    Console.WriteLine("Selamat anda Lulus");
}
else
{
    Console.WriteLine("Anda Tidak Lulus");
}
//STRING AND CHARACTER
Console.WriteLine("Masukkan nama anda : ");
string name = Console.ReadLine();

Console.WriteLine($"Hallo {name}");

Console.WriteLine("Siapa nama anda : ");
string namee = Console.ReadLine();

Console.WriteLine($"Jumlah Karakter anda adalah : {namee.Length}");
