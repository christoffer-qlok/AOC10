namespace Part2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(File.ReadAllLines("input.txt"));
            var zm = map.GetZoomedMap();
            zm.NavigatePipes();
            zm.NavigateConnected(new Coord(0, 0));

            var evenPath = CountEvenPos(zm.Path);
            var evenVisited = CountEvenPos(zm.Visited);
            Console.WriteLine($"zm path: {evenPath}, zm visited: {evenVisited}");

            int result = (map.CharMap.Length * map.CharMap[0].Length) - evenPath - evenVisited;

            Console.WriteLine($"Inner tiles: {result}");
        }

        static int CountEvenPos(HashSet<Coord> list)
        {
            int count = 0;
            foreach (Coord coord in list)
            {
                if(coord.X % 2 == 0 && coord.Y % 2 == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }
}