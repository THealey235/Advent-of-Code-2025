using System.Diagnostics;
using System.Numerics;

namespace Day_08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Vector3> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n")
                    .Select(x => x.Split(','))
                    .Select(x => new Vector3(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                    .ToList();
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

        static long Part1(List<Vector3> input)
        {
            var circuits = Enumerable.Repeat(0, input.Count).ToList();
            var jBoxes = Enumerable.Range(0, input.Count).Select(x => new JBox(x)).ToList();
            var distances = FindDistances(input);

            var connections = 0;
            var k = 0;

            while (connections < 1000) //modify this condition based on your input
            {
                var connection = distances[k];

                if (jBoxes[connection.A].Circuit == jBoxes[connection.B].Circuit)
                {
                    connections++;
                    k++;
                    continue;
                }

                jBoxes[connection.A].AddConnection(connection.B);
                jBoxes[connection.B].AddConnection(connection.A);

                jBoxes[connection.B].SetCircuit(jBoxes, jBoxes[connection.A].Circuit);

                connections++;
                k++;
            }

            jBoxes.ForEach(x => circuits[x.Circuit]++);
            circuits.Sort();

            return (long)circuits[^1] * (long)circuits[^2] * (long)circuits[^3];
        }

        static long Part2(List<Vector3> input)
        {
            var distances = FindDistances(input);
            var indexes = Enumerable.Range(0, input.Count).ToList();

            var it = 0;
            while(indexes.Count > 0)
            {
                indexes.Remove(distances[it].A);
                indexes.Remove(distances[it].B);
                it++;
            }

            return (long)input[distances[it - 1].A].X * (long)input[distances[it - 1].B].X;
        }

        private static List<(int A, int B, float distance)> FindDistances(List<Vector3> input)
        {
            var distances = new List<(int A, int B, float Distance)>();
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                    distances.Add((i, j, Vector3.Distance(input[i], input[j])));
            }

            QuickSort(distances, 0, distances.Count - 1);

            return distances;
        }

        #region Sorting
        static int Partition(List<(int, int, float)> arr, int low, int high)
        {
            double pivot = arr[high].Item3;
            int l = low - 1;

            for (int r = low; r < high; r++)
            {
                if (arr[r].Item3 < pivot)
                {
                    l++;
                    (arr[r], arr[l]) = (arr[l], arr[r]);
                }
            }

            (arr[l + 1], arr[high]) = (arr[high], arr[l + 1]);
            return l + 1;
        }

        static void QuickSort(List<(int, int, float)> arr, int low, int high)
        {
            if (low < high)
            {
                var pivotIndex = Partition(arr, low, high);

                QuickSort(arr, low, pivotIndex - 1);
                QuickSort(arr, pivotIndex + 1, high);
            }
        }
        #endregion
    }

    public class JBox
    {
        private List<int> _connections = new List<int>();
        private int _circuit;

        public int Circuit
        {
            get {  return _circuit; }
        }

        public JBox(int index)
        {
            _circuit = index; //so that each JBox starts in a unique circuit
        }

        public void AddConnection(int index)
        {
            _connections.Add(index);
        }

        public void SetCircuit(List<JBox> jboxes, int circuit)
        {
            _circuit = circuit;

            foreach (var connection in _connections)
            {
                if (!(jboxes[connection].Circuit == circuit))  
                    jboxes[connection].SetCircuit(jboxes, circuit);
            }            
        }
    }
}
