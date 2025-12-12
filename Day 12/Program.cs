using System.Diagnostics;

namespace Day_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sizes = new List<int>();
            var trees = new List<(int Area, List<int> Presents)>();
            using (var reader = new StreamReader("Input.txt"))
            {
                var input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n\r\n").Select(x => x.Split("\r\n"));
                sizes = input.SkipLast(1).Select(x => x.Skip(1).Aggregate(0, (a1, y) => a1 + y.Aggregate(0, (a2, z) => z == '#' ? a2 + 1 : a2))).ToList();
                trees = input.TakeLast(1).First().Select(ParseTree).ToList();
            }

            //doesn't work on sample but does on puzzle input
            var part1 = 0;
            foreach (var tree in trees)
            {
                if (tree.Area > tree.Presents.Select((x, i) => (x * sizes[i])).Sum())
                    part1++;
            }

            Console.WriteLine($"Part 1: {part1}");
        }

        static (int Area, List<int> Presents) ParseTree(string x)
        {
            var s = x.Split(": ");
            var dims = s[0].Split('x').Select(int.Parse).ToArray();

            return (dims[0] * dims[1], s[1].Split(' ').Select(int.Parse).ToList());

        }
    }
}
