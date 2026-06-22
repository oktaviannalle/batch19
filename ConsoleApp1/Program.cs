Console.WriteLine("Masukkan nilai n : ");
int n = Convert.ToInt32(Console.ReadLine());

for (int x = 1; x <=n; x++)

    {
        if (x % 3 == 0 && x % 5 == 0)
        Console.Write("foobar");

        else if(x % 3 == 0)
        Console.Write("foo");

        else if(x % 5 == 0)
        Console.Write("bar");

        else
        Console.Write(x);

        if (x < n) Console.Write(", ");

    }
   