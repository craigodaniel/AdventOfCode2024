using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day16
    {
        // --- Day 16:  ---
        // https://adventofcode.com/2024/day/16
        //
        // Part 1 runtime:
        // Part 2 runtime:
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        private static string fileName = "day16_sample.txt";
        //private static string fileName = "day16_actual.txt";
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