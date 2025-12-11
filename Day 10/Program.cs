using System.Diagnostics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

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
            var Build = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.DenseOfArray;

            foreach (var machine in input)
            {
                //https://www.youtube.com/watch?v=Jqg7JgCwQrk
                //equation: xA = B => ATy = Bt, where y = xT and (matrix)T = matrix.Transpose()
                var y = Build(machine.OrganisedButtons).Transpose().PseudoInverse() * Build(machine.OrganisedJoltages).Transpose();

                //https://stackoverflow.com/questions/38804780/find-integer-solutions-to-a-set-of-equations-with-more-unknowns-than-equations



                output += 1;
            }

            return output;
        }

    }

    public class MachineInfo
    {
        public List<bool> Lights;
        public List<List<int>> Buttons;
        public List<int> Joltages;

        public double[,] OrganisedButtons
        {
            get
            {
                var length = Buttons.Aggregate(0, (a, x) => x.Max() > a ? x.Max() : a) + 1;
                var buttons = new double[Buttons.Count, length];

                for (int i = 0; i < length; i++)
                    Buttons[i].ForEach(x => buttons[x, i] = 1);

                return buttons;
            }
        }

        public double[,] OrganisedJoltages
        {
            get
            {
                var joltages = new double[1, Joltages.Count];
                for (int i = 0; i < Joltages.Count; i++)
                    joltages[0, i] = Joltages[i];

                return joltages;
            }
        }

        public MachineInfo(string line)
        {
            var sections = line.Split(' ');
            Lights = sections[0].Substring(1, sections[0].Length - 2).Select(x => x == '#' ? true : false).ToList();

            Buttons = new List<List<int>>();
            for (int i = 1; i < sections.Length - 1; i++)
                Buttons.Add(sections[i].Substring(1, sections[i].Length - 2).Split(',').Select(int.Parse).ToList());

            Joltages = sections[^1].Substring(1, sections[^1].Length - 2).Split(',').Select(int.Parse).ToList();
        }
    }
}
