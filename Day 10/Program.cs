using System.Diagnostics;
using Microsoft.Z3;

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

        //this would have probably been easier in python using the official tutorial
        static long Part2(List<MachineInfo> input)
        {
            long output = 0;
            var processed = 0;
            
            foreach (var machine in input)
            {
                using (Context c = new Context())
                {
                    Solver solver = c.MkSolver();
                    var zero = c.MkInt(0);

                    //setting up coefficients (how many times a button is pressed) 
                    var presses = new IntExpr[machine.Buttons.Count];
                    for (int i = 0; i < machine.Buttons.Count; i++)
                        presses[i] = c.MkIntConst($"x{i}");

                    //setting equations describing which joltages a button increases
                    var equations = new List<IntExpr[]>();
                    for (int i = 0; i < machine.Joltages.Count; i++)
                    {
                        var tempList = new List<IntExpr>();
                        for (int j = 0; j < machine.Buttons.Count; j++)
                        {
                            if (machine.Buttons[j].Contains(i))
                                tempList.Add(presses[j]);
                        }
                        equations.Add(tempList.ToArray());
                    }

                    for (int i = 0; i < machine.Joltages.Count; i++)
                        solver.Add(c.MkEq(c.MkAdd(equations[i]), c.MkInt(machine.Joltages[i])));

                    //restrict domains to non-negative integers
                    foreach (var variable in presses)
                        solver.Add(c.MkGe(variable, zero));

                    var solutions = new List<List<int>>();
                    while (solver.Check() == Status.SATISFIABLE)
                    {
                        var solution = new List<int>();
                        var model = solver.Model;
                        foreach (var press in presses)
                            solution.Add(int.Parse(model.Evaluate(press).ToString()));

                        solutions.Add(solution);

                        //block this solution
                        var blocks = new BoolExpr[solution.Count];
                        for (int i = 0; i < solution.Count; i++)
                            blocks[i] = c.MkNot(c.MkEq(presses[i], c.MkInt(solution[i])));
                        solver.Add(c.MkOr(blocks));
                    }

                    output += solutions.Min(x => x.Sum());
                    processed++;
                    Console.WriteLine($"Processed: {processed}/{input.Count}");
                }
            }

            return output;
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

            Joltages = sections[^1].Substring(1, sections[^1].Length - 2).Split(',').Select(int.Parse).ToList();
        }
    }
}
