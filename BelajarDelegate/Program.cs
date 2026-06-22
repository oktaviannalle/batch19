delegate int Hitung(int x);

class Program
{
    static int Kali2(int x) => x * 2;
    static int Kali3(int x) => x * 3;

    static void Main()
    {
        // === BASIC DELEGATE ===
        Hitung h = Kali2;
        Console.WriteLine(h(5));  // → 10

        h = Kali3;
        Console.WriteLine(h(5));  // → 15

        // === FUNC & ACTION ===
        Func<int, int> kali2 = x => x * 2;
        Console.WriteLine(kali2(5));

        Action<string> sapa = x => Console.WriteLine("Halo " + x);
        sapa("Okta");

        // === EVENT HANDLER ===
        Console.WriteLine("\n Event: Basic ");
        Bel b = new Bel();
        b.Berdering += () => Console.WriteLine("Bel berbunyi!");
        b.Tekan(); // → Bel berbunyi!
    }
}

// === CLASS EVENT ===
class Bel
{
    public event Action Berdering;
    public void Tekan() => Berdering?.Invoke();
}
