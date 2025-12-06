using System.Diagnostics;

namespace Day_06
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
            var output = Part1(new List<string>(input));
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = Part2(input);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static long Part1(List<string> input)
        {
            //formatting input
            var operators = input.TakeLast(1).ToArray()[0].Replace(" ", "");
            input.RemoveAt(input.Count - 1);
            var stringNumbers = input.Select(x => x.Split(' ').ToList()).ToList();
            stringNumbers.ForEach(y => y.RemoveAll(x => x == ""));
            var numbers = stringNumbers.Select(x => x.Select(int.Parse).ToList()).ToList();

            //doing the homework
            long output = 0;

            for (int i = 0; i < numbers[0].Count; i++)
            {
                if (operators[i] == '+')
                    output += numbers.Aggregate(0L, (a, list) => a + list[i]);
                else output += numbers.Aggregate(1L, (a, list) => a * list[i]);
            }

            return output;
        }
        static long Part2(List<string> input)
        {
            //formatting input
            var operators = input.TakeLast(1).ToArray()[0].Replace(" ", "");
            input.RemoveAt(input.Count - 1);

            //"rotating" numbers
            var stringNumbers = Enumerable.Repeat(String.Empty, input[0].Length).ToList();

            foreach (var line in input)
            {
                for (int i = 0; i < stringNumbers.Count; i++)
                    stringNumbers[i] += line[i];
            }
            
            List<List<int>> numbers = new List<List<int>>() { new List<int>()};
            var empty = new string(Enumerable.Repeat(' ', input.Count).ToArray());
            foreach (var number in stringNumbers)
            {
                if (number != empty)
                    numbers[^1].Add(int.Parse(number));
                else numbers.Add(new List<int>());
            }

            //doing the homework
            long output = 0;

            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] == '+')
                    output += numbers[i].Aggregate(0L, (a, x) => a + x);
                else output += numbers[i].Aggregate(1L, (a, x) => a * x);
            }

            return output;
        }
    }
}
