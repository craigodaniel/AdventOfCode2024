using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day24
    {
        // --- Day 24:  ---
        // https://adventofcode.com/2024/day/24
        //
        // Part 1 runtime:
        // Part 2 runtime:
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        private static string fileName = "day24_sample.txt";
        //private static string fileName = "day24_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }
    }
}