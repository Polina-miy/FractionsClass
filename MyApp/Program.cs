class Program
{
    static void Main()
    {
        Fractions a = new Fractions(3, 2500);
        Fractions b = new Fractions(1, 8000);

        Fractions sum = a + b;
        Fractions diff = a - b;
        Fractions mul = a * b;

        Console.WriteLine("A = " + a);
        Console.WriteLine("B = " + b);

        Console.WriteLine("A + B = " + sum);
        Console.WriteLine("A - B = " + diff);
        Console.WriteLine("A * B = " + mul);

        Console.WriteLine("A > B: " + (a > b));
        Console.WriteLine("A < B: " + (a < b));
        Console.WriteLine("A == B: " + (a == b));
    }
}
