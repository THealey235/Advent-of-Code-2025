using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Day_07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<char[]> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n").Select(x => x.ToArray()).ToList();
            }

            //changing S to |
            input[0][(input[0].Length - 1) / 2] = '|';

            var watch = new Stopwatch();

            watch.Start();
            long output = Part1(input);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = Part2(input);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static long Part1(List<char[]> input)
        {
            var output = 0;
            for (int row = 0; row < input.Count - 1; row++)
            {
                for (int col = 0; col < input[0].Length; col++)
                {
                    if (input[row][col] != '|')
                        continue;

                    if (input[row + 1][col] == '^')
                    {
                        if (input[row + 1][col - 1] != '|' || input[row + 1][col + 1] != '|')
                            output++;

                        input[row + 1][col - 1] = input[row + 1][col + 1] = '|';
                    }
                    else
                        input[row + 1][col] = '|';
                }
            }

            return output;
        }

        static long Part2(List<char[]> input)
        {
            return FollowPath(input, 0, (input.Count - 1) / 2);
        }

        static Dictionary<(int row, int col), long> Memo = new Dictionary<(int row, int col), long>();

        static long FollowPath(List<char[]> input, int row, int col)
        {
            if (row + 2 == input.Count)
                return 1;
            else
            {
                if (input[row + 1][col] == '^')
                {
                    long x;
                    if (Memo.TryGetValue((row, col), out x))
                        return x;

                    long l;
                    long r;

                    if (!Memo.TryGetValue((row + 1, col - 1), out l))
                        l = FollowPath(input, row + 1, col - 1);
                    if (!Memo.TryGetValue((row + 1, col + 1), out r))
                        r = FollowPath(input, row + 1, col + 1);

                    x = l + r;
                    Memo.Add((row, col), x);
                    return x;
                }
                else
                    return FollowPath(input, row + 1, col);
            }
        }
    }
}
