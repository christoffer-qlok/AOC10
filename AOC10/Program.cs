namespace AOC10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(File.ReadAllLines("input.txt"));

            int steps = map.NavigatePipes();

            Console.WriteLine($"Steps: {steps}");
            Console.WriteLine($"Furthest point: {steps/2}");
        }
    }
}