using System.Diagnostics;

namespace Day_09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<(int Row, int Col)> input;
            using (var reader = new StreamReader("Input.txt"))
            {
                input = reader.ReadToEnd().ReplaceLineEndings().Split("\r\n")
                        .Select(x => x.Split(','))
                        .Select(x => (int.Parse(x[0]), int.Parse(x[1])))
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

        static long Part1(List<(int Row, int Col)> input)
        {
            long largestArea = 0;
            long area = 0;

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    area = Math.Abs((long)(input[i].Row - input[j].Row + 1) * (long)(input[i].Col - input[j].Col + 1));
                    if (area > largestArea)
                        largestArea = area;
                }
            }

            return largestArea;
        }
        static long Part2(List<(int X, int Y)> input)
        {
            var horizontalLines = new List<(int Start, int End, int Y)>();
            var verticalLines = new List<(int Start, int End, int X)>();
            int upper;
            int lower;

            input.Add(input[0]);
            //define edges of the floor plan created by the tiles
            for (int i = 0; i < input.Count - 1; i++)
            {
                if ((input[i].X == input[i + 1].X))
                {
                    if (input[i].Y > input[i + 1].Y)
                    {
                        upper = input[i].Y;
                        lower = input[i + 1].Y;
                    }
                    else 
                    {
                        upper = input[i + 1].Y;
                        lower = input[i].Y; 
                    }
                    verticalLines.Add((lower, upper, input[i].X));
                }
                else
                {
                    if (input[i].X > input[i + 1].X)
                    {
                        upper = input[i].X;
                        lower = input[i + 1].X;
                    }
                    else
                    {
                        upper = input[i + 1].X;
                        lower = input[i].X;
                    }
                    horizontalLines.Add((lower, upper, input[i].Y));
                }
            }
            input.RemoveAt(input.Count - 1);

            long largestArea = 0;
            long area = 0;
            bool isBroken;
            Rectangle rectangle;

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = i + 2; j < input.Count; j++)
                {
                    isBroken = false;
                    rectangle = new Rectangle(input[i], input[j]);
                    foreach (var line in horizontalLines)
                    {
                        //if the line is intersects the edge of the rectangle
                        if ((line.Y > rectangle.Bottom && line.Y < rectangle.Top) &&
                            (line.Start < rectangle.Left && line.End > rectangle.Right ||
                            line.Start < rectangle.Right && line.End > rectangle.Left))
                        {
                            isBroken = true;
                            break;
                        }
                    }

                    if (isBroken)
                        continue;

                    foreach (var line in verticalLines)
                    {
                        if ((line.X < rectangle.Right && line.X > rectangle.Left) &&
                            (line.Start < rectangle.Top && line.End > rectangle.Bottom ||
                            line.Start < rectangle.Bottom && line.End > rectangle.Top))
                        {
                            isBroken = true;
                            break;
                        }
                    }

                    if (isBroken)
                        continue;

                    area = rectangle.Area;
                    if (area > largestArea)
                        largestArea = area;
                }
            }

            return largestArea;
        }
    }

    public class Rectangle
    {
        public int Top;
        public int Bottom;
        public int Right;
        public int Left;

        public long Area
        {
            get { return (long)(Top - Bottom + 1) * (long)(Right - Left + 1); }
        }

        public Rectangle((int X, int Y) a, (int X, int Y) b)
        {
            if (a.Y > b.Y)
            {
                Top = a.Y;
                Bottom = b.Y;
            }
            else
            {
                Top = b.Y;
                Bottom = a.Y;
            }

            if (a.X > b.X)
            {
                Right = a.X;
                Left = b.X;
            }
            else
            {
                Right = b.X;
                Left = a.X;
            }
        }
    }
}
