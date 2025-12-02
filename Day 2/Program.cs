using System.Diagnostics;

namespace Day_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().Split(',').ToList();
            }
            var watch = new Stopwatch();

            watch.Start();
            var output = Part1(input);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMilliseconds} ms");

            watch.Restart();
            output = Part2(input);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalSeconds} s");
        }

        static long Part1(List<string> input)
        {
            var ranges = input.Select(x => x.Split('-').Select(x => long.Parse(x)).ToArray()).ToArray();
            long output = 0;
            int chunkLength;
            string ID;

            foreach (var range in ranges)
            {;
                for (long pID = range[0]; pID <= range[1]; pID++)
                {
                    ID = pID.ToString();
                    if (ID.Length % 2 == 1) //if it has an odd number of chars it cannot fit the pattern
                        continue;

                    chunkLength = ID.Length / 2;
                    if (new string(ID.Take(chunkLength).ToArray()) == new string(ID.Skip(chunkLength).Take(chunkLength).ToArray()))
                        output += pID;
                }
            }

            return output;
        }

        static long Part2(List<string> input)
        {
            var ranges = input.Select(x => x.Split('-').Select(x => long.Parse(x)).ToArray()).ToArray();
            long output = 0;
            string ID;

            foreach (var range in ranges)
            {
                for (long pID = range[0]; pID <= range[1]; pID++)
                {
                    ID = pID.ToString();
                    for (int i = ID.Length / 2; i > 0; i--)
                    {
                        if (ID.Chunk(i).Select(x => new string(x)).ToArray().Distinct().Count() != 1)
                            continue;
                        output += pID;
                        break;
                    }
                }
            }

            return output;
        }

    }
}
