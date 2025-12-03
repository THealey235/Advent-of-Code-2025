using System.Diagnostics;
using System.Text.Json;

namespace Day_03
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

            long output;

            watch.Start();
            output = FindJolts(input, 2);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = FindJolts(input, 12);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static long FindJolts(List<string> banks, int batteries)
        {
            var digits = Enumerable.Range(1, 9).Select(x => x.ToString()[0]).ToArray();
            long output = 0;
            List<char> bankList;
            foreach (var bank in banks)
            {
                int[] jolts = Enumerable.Repeat(0, batteries).ToArray();

                int left = 0;
                int right = batteries;
                int a;

                for (int i = 0; i < batteries; i++)
                {
                    right--;
                    bankList = bank.Substring(left, bank.Length - right - left).ToList();
                    a = 0;
                    for (int j = 8; j >= 0; j--)
                    {
                        a = bankList.IndexOf(digits[j]);
                        if (a != -1)
                        {
                            jolts[i] = bankList[a] - '0';
                            break;
                        }
                    }
                    left += a + 1;
                    
                }
                output += long.Parse(String.Join(String.Empty, jolts));
            }

            return output;
        }
    }
}
