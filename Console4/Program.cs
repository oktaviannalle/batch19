Console.WriteLine("Masukkan nilai n : ");
int n = Convert.ToInt32(Console.ReadLine());

for(int x = 1; x <= n; x++)
{
    string output = "";

    if(x % 3 == 0)
    output += "foo";

    if(x % 4 == 0)
    output += "baz";

    if (x % 5 == 0)
    output +="bar";

    if(x % 7 == 0)
    output += "jazz";

    if(x % 9 == 0)
    output += "huzz";

    if (output == "")
    output = x.ToString();

    Console.Write(output);

    if (x < n)
    Console.Write(",");

}