using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day02
    {
        // --- Day 2: Red-Nosed Reports ---
        // https://adventofcode.com/2024/day/2
        //
        // Part One: 442 (runtime 0.6216ms)
        // Part Two: 493 (runtime 3.5323ms)
        //
        // Comments: Decided to just brute force Part 2 instead of figuring out a clever algorithm.
        //           Seems to work just fine with the small input size. I'm sure that will change
        //           as the event continues!

        public static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //public static string fileName = "day02_sample.txt";
        public static string fileName = "day02_actual.txt";
        public static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        
        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            lines = AocLib.CleanInput(lines);
            int answer = 0;

            foreach (string line in lines)
            {
                //Initial Conditions
                int[] report = AocLib.GetIntArrayFromString(line);
                bool[] testResults = TestPair(report[0], report[1]); //{isOutofRange, isIncr}
                bool isSafe = !testResults[0]; //safe if not out of range
                bool isIncr = testResults[1];


                int i = 1;//starts on 2nd index

                while (isSafe & i < report.Length - 1)
                {
                    testResults = TestPair(report[i], report[i + 1]);
                    if (isIncr != testResults[1])
                    {
                        isSafe = false;
                        break;
                    }
                    
                    isSafe = !testResults[0];

                    i++;
                }

                if (isSafe)
                {
                    answer++;
                }
                
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Runtime: " + elapsedTime.TotalMilliseconds + "ms");
            Console.WriteLine("Part One Answer:");
            Console.WriteLine(answer);

        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            lines = AocLib.CleanInput(lines);
            int answer = 0;

            foreach (string line in lines)
            {
                //Initial Conditions
                int[] report = AocLib.GetIntArrayFromString(line);
                
                bool isSafe = CheckReportIsSafe(report);
                bool isSafeWithProblemDampner = false;

                if (isSafe) {isSafeWithProblemDampner = true;}
                else
                {
                    for (int omitIndex = 0; omitIndex < report.Length; omitIndex++)
                    {
                        int[]altReport = new int[report.Length -1];
                        int x = 0;
                        for (int i = 0; i < report.Length; i++)
                        {
                            if (i != omitIndex)
                            {
                                altReport[x] = report[i];
                                x++;
                            }
                        }

                        isSafe = CheckReportIsSafe(altReport);
                        if (isSafe)
                        {
                            isSafeWithProblemDampner = true;
                            omitIndex = report.Length;
                        }
                    }
                }

                if (isSafeWithProblemDampner) {answer++;}
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Runtime: " + elapsedTime.TotalMilliseconds + "ms");
            Console.WriteLine("Part One Answer:");
            Console.WriteLine(answer);
        }


        private static bool[] TestPair(int firstInt, int secondInt)
        {
            bool isOutOfRange = false;
            bool isIncr = false;

            int diff = secondInt - firstInt;
            int dist = Math.Abs(diff);

            if (dist < 1 || dist > 3)
            {
                isOutOfRange = true;
            }

            if (diff > 0)
            {
                isIncr = true;
            }


            return new bool[] { isOutOfRange, isIncr };

        }

        private static bool CheckReportIsSafe(int[] report)
        {
            //Initial Conditions
            bool[] testResults = TestPair(report[0], report[1]); //{isOutofRange, isIncr}
            bool isSafe = !testResults[0]; //safe if not out of range
            bool isIncr = testResults[1];

            int i = 1;//starts on 2nd index

            while (isSafe & i < report.Length - 1)
            {
                testResults = TestPair(report[i], report[i + 1]);
                if (isIncr != testResults[1])
                {
                    isSafe = false;
                    break;
                }

                isSafe = !testResults[0];

                i++;
            }

            return isSafe;
        }
    }
}
