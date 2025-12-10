using System.Diagnostics;

namespace Day_05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<long> IDs;
            List<Range> ranges;

            using (var reader = new StreamReader("Input.txt"))
            {
                var input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n\r\n");
                IDs = input[1].Split("\r\n").Select(long.Parse).ToList();
                ranges = input[0].Split("\r\n").Select(x => x.Split('-')
                                 .Select(y => long.Parse(y)).ToArray())
                                 .Select(x => new Range(x[0], x[1])).ToList();

            }
            var watch = new Stopwatch();

            watch.Start();
            var output = Part1(ranges, IDs);
            watch.Stop();
            Console.WriteLine($"Part 1: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");

            watch.Restart();
            output = Part2(ranges);
            watch.Stop();
            Console.WriteLine($"Part 2: {output}, Time: {watch.Elapsed.TotalMicroseconds} μs");
        }

        static long Part1(List<Range> ranges, List<long> IDs)
        {
            long output = 0;

            foreach (var ID in IDs)
            {
                foreach (var range in ranges)
                {
                    if (ID < range.Min || ID > range.Max)
                        continue;
                    output += 1;
                    break;
                }
            }

            return output;
        }
        static long Part2(List<Range> ranges)
        {
            long output = 0;
            bool addRange;
            List<Range> usedRanges = new List<Range>();

            QuickSort(ranges, 0, ranges.Count - 1); //sort ranges by their minimum value

            foreach (var range in ranges)
            {
                addRange = true;
                foreach (var usedRange in usedRanges)
                {
                    //Since the ranges are sorted based on their minimum value
                    if (usedRange.Max > range.Max)
                    {
                        addRange = false;
                        break;
                    } 
                    if (usedRange.Max >= range.Min)
                        range.Min = usedRange.Max + 1;
                }

                if (addRange)
                {
                    usedRanges.Add(range);
                    output += range.Max - range.Min + 1;
                }
            }

            return output;
        }

        static int Partition(List<Range> arr, int low, int high)
        {
            var pivot = arr[high].Min;

            var i = low - 1;
            for (int j = low; j < high; j++)
            {
                if ((arr[j].Min) <= pivot)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }

            Swap(arr, i + 1, high);
            return i + 1;
        }

        static void Swap(List<Range> arr, int i, int j)
        {
            var temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        static void QuickSort(List<Range> arr, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(arr, low, high);

                QuickSort(arr, low, pivot - 1);
                QuickSort(arr, pivot + 1, high);

            }
        }
    }

    public class Range
    {
        public long Min;
        public long Max;

        public Range(long min, long max) { Min = min; Max = max; }
    }
}
