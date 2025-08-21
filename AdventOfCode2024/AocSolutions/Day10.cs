using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day10
    {
        // --- Day 10:  ---
        // https://adventofcode.com/2024/day/10
        //
        // Part 1 runtime: 3.8469ms. The answer is: 482
        // Part 2 runtime: 4.0609ms. The answer is: 1094
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day10_sample.txt";
        private static string fileName = "day10_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static List<Tuple<int, int>> _visitedCells = new List<Tuple<int, int>>();
        private static int _trailCount = 0;
        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            int[,] map = AocLib.GetInt2dArrayFromStringArray(lines);
            List<Tuple<int, int>> startingPosList = GetStartingPosList(map);

            foreach (Tuple<int, int> pos in startingPosList)
            {
                int row = pos.Item1;
                int col = pos.Item2;
                List<Tuple<int, int>> trail = new List<Tuple<int, int>>();
                WalkTrail(map, row, col, trail);

                
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, _trailCount);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            int[,] map = AocLib.GetInt2dArrayFromStringArray(lines);
            List<Tuple<int, int>> startingPosList = GetStartingPosList(map);

            foreach (Tuple<int, int> pos in startingPosList)
            {
                int row = pos.Item1;
                int col = pos.Item2;
                List<Tuple<int, int>> trail = new List<Tuple<int, int>>();
                WalkTrail2(map, row, col);
                

            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, _trailCount);
        }

        public static List<Tuple<int, int>> GetStartingPosList(int[,] map)
        {
            List<Tuple<int, int>> startingPosList = new List<Tuple<int, int>>();

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(0); col++)
                {
                    if (map[row, col] == 0)
                    {
                        startingPosList.Add(new Tuple<int, int>(row, col));
                    }
                }
            }

            return startingPosList;
        }

        public static void WalkTrail(int[,] map, int row, int col, List<Tuple<int,int>> trail)
        {
            
            if (trail.Contains(new Tuple<int, int>(row, col)))
            {
                return; // Already visited this cell in the current trail
            }

            trail.Add(new Tuple<int, int>(row, col));

            //if (trail.Count > 10) { return; } // Limit trail length for testing

            if (map[row, col] == 9)
            {
                _trailCount++;
                Console.WriteLine("Trail {0}: {1}", _trailCount, string.Join(" -> ", trail.Select(t => $"({t.Item1},{t.Item2})")));
                return; // Reached the end of a valid trail
            }

            int[][] directions = new int[][]
            {
                new int[] {-1, 0}, // Up
                new int[] {1, 0},  // Down
                new int[] {0, -1}, // Left
                new int[] {0, 1}   // Right
            };
            foreach (int[] direction in directions)
            {
                int neighborValue = CheckNeighbor(map, row, col, direction);
                if (neighborValue != -1 && neighborValue == map[row, col] + 1)
                {
                    WalkTrail(map, row + direction[0], col + direction[1], trail);
                }
            }

        }

        public static void WalkTrail2(int[,] map, int row, int col)
        {
            //if (trail.Count > 10) { return; } // Limit trail length for testing

            if (map[row, col] == 9)
            {
                _trailCount++;
                return; // Reached the end of a valid trail
            }

            int[][] directions = new int[][]
            {
                new int[] {-1, 0}, // Up
                new int[] {1, 0},  // Down
                new int[] {0, -1}, // Left
                new int[] {0, 1}   // Right
            };
            foreach (int[] direction in directions)
            {
                int neighborValue = CheckNeighbor(map, row, col, direction);
                if (neighborValue != -1 && neighborValue == map[row, col] + 1)
                {
                    WalkTrail2(map, row + direction[0], col + direction[1]);
                }
            }

        }

        public static int CheckNeighbor(int[,] map, int row, int col, int[] direction)
        {
            if (row + direction[0] < 0 || row + direction[0] >= map.GetLength(0) ||
                col + direction[1] < 0 || col + direction[1] >= map.GetLength(1))
            {
                return -1; // Out of bounds
            }
            else
            {
                // Return the value of the neighbor cell
                return map[row + direction[0], col + direction[1]];
            }
        }
    }
}