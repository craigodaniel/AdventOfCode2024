using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day12
    {
        // --- Day 12:  ---
        // https://adventofcode.com/2024/day/12
        //
        // Part 1 runtime: 17.4106ms. The answer is: 1452678
        // Part 2 runtime: 14.3344ms. The answer is: 873584
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day12_sample.txt";
        private static string fileName = "day12_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static List<List<(int, int)>> regions = new List<List<(int, int)>>();

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            FindRegions();
            foreach (var region in regions)
            {
                int area = region.Count;
                int perimeter = GetPerimeter(region);
                int score = area * perimeter;
                answer += score;
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            FindRegions();
            foreach (var region in regions)
            {
                int area = region.Count;
                int corners = CountCorners(region);
                int score = area * corners;
                //Console.WriteLine($"Region with area {area} has {corners} corners for score {score}");
                answer += score;
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }


        private static void FindRegions()
        {
            int numRows = lines.Length;
            int numCols = lines[0].Length;
            bool[,] visited = new bool[numRows, numCols];
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    if (!visited[r, c])
                    {
                        List<(int, int)> region = new List<(int, int)>();
                        ExploreRegion((r, c), visited, region);
                        regions.Add(region);
                    }
                }
            }
        }

        private static void ExploreRegion((int, int) startCell, bool[,] visited, List<(int, int)> region)
        {
            int numRows = visited.GetLength(0);
            int numCols = visited.GetLength(1);
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push(startCell);
            visited[startCell.Item1, startCell.Item2] = true;
            while (stack.Count > 0)
            {
                var cell = stack.Pop();
                region.Add(cell);
                for (int direction = 0; direction < 4; direction++)
                {
                    var neighbor = GetNeighbor(cell, direction);
                    if (neighbor.Item1 >= 0 && neighbor.Item1 < numRows &&
                        neighbor.Item2 >= 0 && neighbor.Item2 < numCols &&
                        !visited[neighbor.Item1, neighbor.Item2] &&
                        lines[neighbor.Item1][neighbor.Item2] == lines[cell.Item1][cell.Item2])
                    {
                        visited[neighbor.Item1, neighbor.Item2] = true;
                        stack.Push(neighbor);
                    }
                }
            }
        }
        private static bool IsNeighborInRegion((int, int) neighbor, List<(int, int)> region)
        {
            return region.Contains(neighbor);
        }

        private static (int, int) GetNeighbor((int, int) cell, int direction)
        {
            // direction: 0 = up, 1 = right, 2 = down, 3 = left
            return direction switch
            {
                0 => (cell.Item1 - 1, cell.Item2),
                1 => (cell.Item1, cell.Item2 + 1),
                2 => (cell.Item1 + 1, cell.Item2),
                3 => (cell.Item1, cell.Item2 - 1),
                _ => throw new ArgumentException("Invalid direction"),
            };
        }

        private static int GetPerimeter(List<(int, int)> region)
        {
            int perimeter = 0;
            HashSet<(int, int)> regionSet = new HashSet<(int, int)>(region);
            foreach (var cell in region)
            {
                for (int direction = 0; direction < 4; direction++)
                {
                    var neighbor = GetNeighbor(cell, direction);
                    if (!regionSet.Contains(neighbor))
                    {
                        perimeter++;
                    }
                }
            }
            return perimeter;
        }

        private static int CountCorners(List<(int, int)> region) 
        {
            int corners = 0;
            HashSet<(int, int)> regionSet = new HashSet<(int, int)>(region);
            foreach (var cell in region)
            {
                if (!regionSet.Contains(GetNeighbor(cell, 0)) && !regionSet.Contains(GetNeighbor(cell, 1)))
                {
                    corners++;
                }
                if (!regionSet.Contains(GetNeighbor(cell, 1)) && !regionSet.Contains(GetNeighbor(cell, 2)))
                {
                    corners++;
                }
                if (!regionSet.Contains(GetNeighbor(cell, 2)) && !regionSet.Contains(GetNeighbor(cell, 3)))
                {
                    corners++;
                }
                if (!regionSet.Contains(GetNeighbor(cell, 3)) && !regionSet.Contains(GetNeighbor(cell, 0)))
                {
                    corners++;
                }

                // Handle concave corners
                if (regionSet.Contains(GetNeighbor(cell, 0)) && regionSet.Contains(GetNeighbor(cell, 3)) && !regionSet.Contains(GetNeighbor(GetNeighbor(cell, 0), 3)))
                {
                    corners++;
                }
                if (regionSet.Contains(GetNeighbor(cell, 0)) && regionSet.Contains(GetNeighbor(cell, 1)) && !regionSet.Contains(GetNeighbor(GetNeighbor(cell, 0), 1)))
                {
                    corners++;
                }
                if (regionSet.Contains(GetNeighbor(cell, 2)) && regionSet.Contains(GetNeighbor(cell, 1)) && !regionSet.Contains(GetNeighbor(GetNeighbor(cell, 2), 1)))
                {
                    corners++;
                }
                if (regionSet.Contains(GetNeighbor(cell, 2)) && regionSet.Contains(GetNeighbor(cell, 3)) && !regionSet.Contains(GetNeighbor(GetNeighbor(cell, 2), 3)))
                {
                    corners++;
                }


            }

            return corners;
        }
    }
}
       