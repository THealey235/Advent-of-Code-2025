using System.Diagnostics;

namespace Day_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n").ToList();
            }
            var watch = new Stopwatch();

            watch.Start();
            var output = Part1(input);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = Part2(input);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static int Mod(int a, int b) => (a % b + b) % b; //replacement for % operator causing off by 1 errors

        static int Part1(List<string> directions)
        {
            var position = 50;
            var output = 0;

            foreach (var direction in directions)
            {
                if (direction[0] == 'R')
                    position += int.Parse(direction.Split('R')[1]);
                else position -= int.Parse(direction.Split('L')[1]);

                position = Mod(position, 100);

                if (position == 0)
                    output++;
            }

            return output;
        }

        static int Part2(List<string> directions)
        {
            var previous = 0;
            var position = 50;
            var output = 0;

            foreach (var direction in directions)
            {
                previous = position;

                if (direction[0] == 'R')
                    position += int.Parse(direction.Split('R')[1]);
                else position -= int.Parse(direction.Split('L')[1]);

                output += Math.Abs(position / 100);

                if (position <= 0 && !(previous == 0))
                    output++;

                position = Mod(position, 100);
            }

            return output;
        }

    }
}
