using System.Diagnostics;

namespace Day_04
{
    internal class Program
    {
        static List<Vector2> Directions = new List<Vector2>()
        {
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
            new Vector2(-1, 0), new Vector2(1, 0),
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1)
        };
        
        static void Main(string[] args)
        {
            char[][] input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n").Select(x => x.ToCharArray()).ToArray();
            }
            var watch = new Stopwatch();

            watch.Start();
            var output = Part1(input, out var x);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = Part2(input);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static int Part1(char[][] map, out List<Vector2> reachableLocations)
        {
            reachableLocations = new List<Vector2>();
            int reachableRolls = 0;
            int adjRolls;

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    adjRolls = 0;
                    if (map[i][j] == '.')
                        continue;

                    foreach (var dir in Directions)
                    {
                        //if it is on the border skip directions which would result in invalid indexes
                        if (i == 0 && dir.Y == -1 || i == map.Length - 1 && dir.Y == 1 ||
                            j == 0 && dir.X == -1 || j == map[0].Length - 1 && dir.X == 1)
                            continue;

                        if (map[i + dir.Y][j + dir.X] == '@')
                            adjRolls++;

                        if (adjRolls > 3)
                            break;
                    }

                    if (adjRolls < 4)
                    {
                        reachableRolls++;
                        reachableLocations.Add(new Vector2(j, i));
                    }
                }
            }

            return reachableRolls;
        }
        static int Part2(char[][] map)
        {
            int reachableRolls = -1;
            int output = 0;
            List<Vector2> reachableLocations;

            while (reachableRolls != 0)
            {
                reachableRolls = Part1(map, out reachableLocations);
                output += reachableRolls;

                foreach (var location in reachableLocations)
                    map[location.Y][location.X] = '.';
            }

            return output;
        }
    }

    public class Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
