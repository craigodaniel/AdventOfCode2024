using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day08
    {
        // --- Day 8: Resonant Collinearity ---
        // https://adventofcode.com/2024/day/8
        //
        // Part 1 runtime: 11.0503ms. The answer is: 256
        // Part 2 runtime: 11.609ms. The answer is: 1005
        //
        // Comments: It's ugly but it works!

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day08_sample.txt";
        private static string fileName = "day08_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static List<Tuple<int, int>> antinodes = new List<Tuple<int, int>>();

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            int lastRow = lines.Length - 1;
            int lastCol = lines[0].Length - 1;
            antinodes.Clear();
            Dictionary<char, List<Tuple<int, int>>> antennas = GetAntennas();

            foreach (char key in antennas.Keys)
            {
                for (int i = 0; i < antennas[key].Count; i++)
                {
                    for (int j = 0; j < antennas[key].Count; j++)
                    {
                        if (i != j)
                        {
                            GetAntinodes(antennas[key][i], antennas[key][j], lastRow, lastCol);
                        }
                    }
                }
            }

            answer = antinodes.Count;

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            int lastRow = lines.Length - 1;
            int lastCol = lines[0].Length - 1;
            antinodes.Clear();
            Dictionary<char, List<Tuple<int, int>>> antennas = GetAntennas();

            foreach (char key in antennas.Keys)
            {
                for (int i = 0; i < antennas[key].Count; i++)
                {
                    for (int j = 0; j < antennas[key].Count; j++)
                    {
                        if (i != j)
                        {
                            GetAntinodesDir1(antennas[key][i], antennas[key][j], lastRow, lastCol);
                            GetAntinodesDir2(antennas[key][i], antennas[key][j], lastRow, lastCol);
                            if (!CheckAntinodesListContains(antennas[key][i]))
                            {
                                antinodes.Add(antennas[key][i]);
                            }
                            if (!CheckAntinodesListContains(antennas[key][j]))
                            {
                                antinodes.Add(antennas[key][j]);
                            }
                        }
                    }
                }
            }

            answer = antinodes.Count;

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static Dictionary<char, List<Tuple<int, int>>> GetAntennas()
        {
            Dictionary<char, List<Tuple<int, int>>> antennas = new Dictionary<char, List<Tuple<int, int>>>();
            char[,] map = AocLib.GetChar2dArrayFromStringArray(lines);

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    char tile = map[row, col];
                    if (tile != '.')
                    {
                        if (!antennas.ContainsKey(map[row, col]))
                        {
                            antennas.Add(map[row, col], new List<Tuple<int, int>>());
                        }
                        antennas[map[row, col]].Add(new Tuple<int, int>(row, col));
                    }
                }
            }

            return antennas;
        }

        private static void GetAntinodes(Tuple<int, int> antenna1, Tuple<int, int> antenna2, int lastRow, int lastCol)
        {
            int x1 = antenna1.Item1;
            int x2 = antenna2.Item1;
            int y1 = antenna1.Item2;
            int y2 = antenna2.Item2;

            int dx = x2 - x1;
            int dy = y2 - y1;

            Tuple<int, int> antinode1 = new Tuple<int, int>(x1 - dx, y1 - dy);
            Tuple<int, int> antinode2 = new Tuple<int, int>(x2 + dx, y2 + dy);

            if (antinode1.Item1 >= 0 && antinode1.Item1 <= lastRow && antinode1.Item2 >= 0 && antinode1.Item2 <= lastCol)
            {
                if (!CheckAntinodesListContains(antinode1))
                {
                    antinodes.Add(antinode1);
                }
            }

            if (antinode2.Item1 >= 0 && antinode2.Item1 <= lastRow && antinode2.Item2 >= 0 && antinode2.Item2 <= lastCol)
            {
                if (!CheckAntinodesListContains(antinode2))
                {
                    antinodes.Add(antinode2);
                }
            }
        }

        private static bool CheckAntinodesListContains(Tuple<int, int> antinode)
        {
            bool isFound = false;

            foreach (Tuple<int, int> pair in antinodes)
            {
                if (pair.Item1 == antinode.Item1 && pair.Item2 == antinode.Item2) { isFound = true; break; }
            }
            return isFound;
        }

        private static void GetAntinodesDir1(Tuple<int, int> antenna1, Tuple<int, int> antenna2, int lastRow, int lastCol)
        {
            int x1 = antenna1.Item1;
            int x2 = antenna2.Item1;
            int y1 = antenna1.Item2;
            int y2 = antenna2.Item2;

            int dx = x2 - x1;
            int dy = y2 - y1;

            Tuple<int, int> antinode1 = new Tuple<int, int>(x1 - dx, y1 - dy);

            if (antinode1.Item1 >= 0 && antinode1.Item1 <= lastRow && antinode1.Item2 >= 0 && antinode1.Item2 <= lastCol)
            {
                if (!CheckAntinodesListContains(antinode1))
                {
                    antinodes.Add(antinode1);
                }
                GetAntinodesDir1(antinode1, antenna1, lastRow, lastCol);
            }

        }

        private static void GetAntinodesDir2(Tuple<int, int> antenna1, Tuple<int, int> antenna2, int lastRow, int lastCol)
        {
            int x1 = antenna1.Item1;
            int x2 = antenna2.Item1;
            int y1 = antenna1.Item2;
            int y2 = antenna2.Item2;

            int dx = x2 - x1;
            int dy = y2 - y1;

            Tuple<int, int> antinode2 = new Tuple<int, int>(x2 + dx, y2 + dy);


            if (antinode2.Item1 >= 0 && antinode2.Item1 <= lastRow && antinode2.Item2 >= 0 && antinode2.Item2 <= lastCol)
            {
                if (!CheckAntinodesListContains(antinode2))
                {
                    antinodes.Add(antinode2);
                }
                GetAntinodesDir2(antenna2, antinode2, lastRow, lastCol);
            }
        }
    }
}