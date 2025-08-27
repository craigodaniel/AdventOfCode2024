using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day13
    {
        // --- Day 13:  ---
        // https://adventofcode.com/2024/day/13
        //
        // Part 1 runtime: 7.8628ms. The answer is: 29598
        // Part 2 runtime: 2.9876ms. The answer is: 93217456941970
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day13_sample.txt";
        private static string fileName = "day13_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            for (int i = 0; i < lines.Length;)
            {
                string lineA= lines[i];
                string lineB = lines[i + 1];
                string linePrize = lines[i + 2];

                string[] partsA = lineA.Split(',');
                int aX = int.Parse(partsA[0].Split('+')[1]);
                int aY = int.Parse(partsA[1].Split('+')[1]);

                string[] partsB = lineB.Split(',');
                int bX = int.Parse(partsB[0].Split('+')[1]);
                int bY = int.Parse(partsB[1].Split('+')[1]);

                string[] partsPrize = linePrize.Split(',');
                int prizeX = int.Parse(partsPrize[0].Split('=')[1]);
                int prizeY = int.Parse(partsPrize[1].Split('=')[1]);

                List<(int,int)> validButtonPresses = new List<(int, int)>();

                for (int buttonA = 0; buttonA < 100; buttonA++)
                {
                    for (int buttonB = 0; buttonB < 100; buttonB++) 
                    {
                        int posX = (buttonA * aX) + (buttonB * bX);
                        int posY = (buttonA * aY) + (buttonB * bY);
                        if (posX == prizeX && posY == prizeY)
                        {
                            validButtonPresses.Add((buttonA, buttonB));
                        }
                    }
                }

                int lowestTokens = -1;

                foreach (var buttonPresses in validButtonPresses)
                {
                    int tokens = (buttonPresses.Item1 * 3) + buttonPresses.Item2;
                    if (lowestTokens == -1 || tokens < lowestTokens) lowestTokens = tokens;
                }
                if (lowestTokens != -1)
                {
                    answer += lowestTokens;
                }
                Console.WriteLine($"Lowest tokens for Round {i / 4}: {lowestTokens}");

                i += 4;
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;

            for (int i = 0; i < lines.Length;)
            {
                string lineA = lines[i];
                string lineB = lines[i + 1];
                string linePrize = lines[i + 2];

                string[] partsA = lineA.Split(',');
                int aX = int.Parse(partsA[0].Split('+')[1]);
                int aY = int.Parse(partsA[1].Split('+')[1]);

                string[] partsB = lineB.Split(',');
                int bX = int.Parse(partsB[0].Split('+')[1]);
                int bY = int.Parse(partsB[1].Split('+')[1]);

                string[] partsPrize = linePrize.Split(',');
                long prizeX = long.Parse(partsPrize[0].Split('=')[1]) + 10000000000000;
                long prizeY = long.Parse(partsPrize[1].Split('=')[1]) + 10000000000000;

                (long aButtons, long bButtons) = FindIntersection(aX, aY, bX, bY, prizeX, prizeY);

                if (aButtons < 0 || bButtons < 0)
                {
                    aButtons = 0;
                    bButtons = 0;
                }
                if ((aButtons * aX) + (bButtons * bX) != prizeX || (aButtons * aY) + (bButtons * bY) != prizeY)
                {
                    aButtons = 0;
                    bButtons = 0;
                }
                long lowestTokens = (aButtons * 3) + bButtons;
                answer += lowestTokens;


                //Console.WriteLine($"Lowest tokens for Round {i / 4}: {lowestTokens}");

                i += 4;
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static (long, long) FindIntersection(int aX, int aY, int bX, int bY, long prizeX, long prizeY)
        {
            long aButtons = 0;
            long bButtons = 0;

            long aX_bY = aX * bY;
            long prizeX_bY = prizeX * bY;
            long aY_bX = aY * bX;
            long prizeY_bX = prizeY * bX;

            aButtons = (prizeX_bY - prizeY_bX) / (aX_bY - aY_bX);
            bButtons = (prizeY - (aButtons * aY)) / bY;


            return (aButtons, bButtons);
        }

    }
}