namespace LogicExercise
{
       class Program
    {
        static void Main(string[]args)
        {
            Console.WriteLine("Masukkan nilai n : ");
            int n = Convert.ToInt32(Console.ReadLine());

            FizzBuzzGenerator myClass = new FizzBuzzGenerator();

            myClass.AddRule(3, "foo");
            myClass.AddRule(4, "baz");
            myClass.AddRule(5, "bar");
            myClass.AddRule(7, "jazz");
            myClass.AddRule(9, "huzz");

            myClass.Generate(n);
        }
    }


    public class FizzBuzzGenerator
    {
    private Dictionary<int,string>_rules = new Dictionary<int, string>();
    public void AddRule(int input,string output)
    {
        _rules[input] = output;
    }

   public void Generate(int n)//method
    {
        for (int x = 1; x <= n; x++)
        {
            string output ="";

            foreach (var rule in _rules)
            {
                if (x % rule.Key == 0)
                {
                    output += rule.Value;
                }
            }

            if (output == "")
            {
                output = x.ToString();
            }
            Console.Write(output);
            if (x < n)
            {
                Console.Write(", ");
            }
        }
        Console.WriteLine();
    } 
    }
} //
    