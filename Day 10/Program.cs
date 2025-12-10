using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Day_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<MachineInfo> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n").Select(x => new MachineInfo(x)).ToList();
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

        static long Part1(List<MachineInfo> input)
        {
            var nextPush = new Queue<(bool[] Lights, int Depth, List<int> Buttons)>();
            long output = 0;

            foreach (var machine in input)
            {
                var deepestFound = 0;
                nextPush.Clear();
                for (int i = 0; i < machine.Buttons.Count; i++)
                    nextPush.Enqueue((Enumerable.Repeat(false, machine.Lights.Count).ToArray(), 0, new List<int>() { i }));
                var it = Enumerable.Range(0, machine.Lights.Count);

                //bfs
                while (true)
                {
                    var order = nextPush.Dequeue();
                    var newLights = order.Lights.Clone() as bool[];
                    machine.Buttons[order.Buttons[^1]].ForEach(x => newLights[x] = !newLights[x]);

                    if (it.Any(x => machine.Lights[x] != newLights[x]))
                    {
                        for (int i = 0; i < machine.Buttons.Count; i++)
                        {
                            if (order.Buttons.Contains(i)) continue;
                            var buttons = new List<int>(order.Buttons);
                            buttons.Add(i);
                            nextPush.Enqueue((newLights, order.Depth + 1, buttons));
                        }
                    }
                    else
                    {
                        deepestFound = order.Depth + 1;
                        break;
                    }
                }

                output += deepestFound;
            }

            return output;
        }

        static long Part2(List<MachineInfo> input)
        {
            long output = 0;
            int maxDepth;
            int i = 1;

            foreach (var machine in input)
            {
                Memo.Clear();
                maxDepth = 0;

                //DFS, because the buttons are sorted so that the buttons with the most conections are at the start they are tried first.
                CheckPath(machine.Joltages, machine.Buttons, Enumerable.Repeat(0, machine.Buttons.Count).ToList(), 0, ref maxDepth);
                Console.WriteLine($"Processed: {i}/{input.Count}");
                i++;
                output += maxDepth;
            }

            return output;
        }

        static List<List<int>> Memo = new List<List<int>>();
        static List<int> target = new List<int>() { 1, 3, 0, 3, 1, 2};

        public static bool CheckPath(List<int> joltages, List<List<int>> buttons, List<int> pressed, int depth, ref int maxDepth)
        {
            var x = 0;
            if (pressed.SequenceEqual(target))
                x = 1;

            foreach (var list in Memo)
            {
                if (list.SequenceEqual(pressed))
                    return false;
            }

            var jolts = Enumerable.Repeat(0, joltages.Count).ToList();
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].ForEach(x => jolts[x] += pressed[i]);
            }

            var flag = true;
            for (int i = 0; i < joltages.Count; i++)
            {
                if (joltages[i] < jolts[i])
                    return false;
                if (joltages[i] > jolts[i])
                    flag = false;
            }

            if (flag)
            {
                maxDepth = depth;
                return true;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                var newPressed = new List<int>(pressed);
                newPressed[i]++;
                if (CheckPath(joltages, buttons, newPressed, depth + 1, ref maxDepth))
                    return true;
                Memo.Add(newPressed);
            }

            return false;
        }

        public static void QuickSort(List<List<int>> buttons, int low, int high)
        {
            if (low < high)
            {
                var p = Partition(buttons, low, high);
                QuickSort(buttons, low, p - 1);
                QuickSort(buttons, p + 1, high);
            }

        }

        public static int Partition(List<List<int>> buttons, int low, int high)
        {
            var pivot = buttons[high].Count;
            var l = low - 1;

            for (int r = low; r < high; r++)
            {
                if (buttons[r].Count >= pivot)
                {
                    l++;
                    (buttons[l], buttons[r]) = (buttons[r], buttons[l]);
                }
            }

            (buttons[l + 1], buttons[high]) = (buttons[high], buttons[l + 1]);
            return l + 1;
        }
    }

    public class MachineInfo
    {
        public List<bool> Lights;
        public List<List<int>> Buttons;
        public List<int> Joltages;

        public MachineInfo(string line)
        {
            var sections = line.Split(' ');
            Lights = sections[0].Substring(1, sections[0].Length - 2).Select(x => x == '#' ? true : false).ToList();

            Buttons = new List<List<int>>();
            for (int i = 1; i < sections.Length - 1; i++)
                Buttons.Add(sections[i].Substring(1, sections[i].Length - 2).Split(',').Select(int.Parse).ToList());

            Program.QuickSort(Buttons, 0, Buttons.Count - 1);

            Joltages = sections[^1].Substring(1, sections[^1].Length - 2).Split(',').Select(int.Parse).ToList();
        }
    }
}
