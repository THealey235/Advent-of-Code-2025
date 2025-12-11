using System.Diagnostics;

namespace Day_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n").Select(x => x.Split(' ')).ToDictionary(x => x[0].Split(':')[0], x => x.Skip(1).ToList());
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

        static Dictionary<string, int> Memo = new Dictionary<string, int>();

        static long Part1(Dictionary<string, List<string>> input) => FollowOutputs(input, "you");

        static int FollowOutputs(Dictionary<string, List<string>> input, string device)
        {
            if (Memo.ContainsKey(device))
                return Memo[device];

            if (device == "out")
                return 1;

            int total = 0;
            foreach (var output in input[device])
                total += FollowOutputs(input, output);

            Memo.Add(device, total);
            return total;
        }

        static long Part2(Dictionary<string, List<string>> input)
        {
            return FollowOutputsPart2(input, "svr", new List<string>(), false, false);
        }

        static Dictionary<(string Device, bool HasDAC, bool HasFFT), long> MemoP2 = new Dictionary<(string, bool HasDAC, bool HasFFT), long>();

        static long FollowOutputsPart2(Dictionary<string, List<string>> input, string device, List<string> path, bool hasDAC, bool hasFFT)
        {
            long total;

            if (MemoP2.TryGetValue((device, hasDAC, hasFFT), out total))
                return total;

            total = 0;

            if (device == "out")
            {
                if (path.Contains("dac") && path.Contains("fft"))
                    return 1;
                else return 0;
            }

            path.Add(device);
            foreach (var output in input[device])
                total += FollowOutputsPart2(input, output, new List<string>(path), hasDAC || output == "dac", hasFFT || output == "fft");

            MemoP2[(device, hasDAC, hasFFT)] = total;
            return total;
        }
    }
}
