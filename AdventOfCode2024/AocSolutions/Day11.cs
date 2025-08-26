using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day11
    {
        // --- Day 11:  ---
        // https://adventofcode.com/2024/day/11
        //
        // Part 1 runtime: 10.0687ms. The answer is: 202019
        // Part 2 runtime: 344.7852ms. The answer is: 239321955280205
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day11_sample.txt";
        private static string fileName = "day11_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static List<long> stones = AocLib.GetListOfLongsFromString(lines[0], " ");
        private static List<long> tempStones = new List<long>();
        private static int skipPosition = -1;
        private static Dictionary<long, long[]> memoization = new Dictionary<long, long[] >();
        private static Dictionary<long, long> stoneCounts = new Dictionary<long, long>();
        private static int blinkCount = 75;
        private static long stoneCount = 0;
        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < stones.Count; j++)
                {
                    long stone = stones[j];
                    int position = j;
                    if (position == skipPosition)
                    {
                        skipPosition = -1;
                        continue; // Skip this position
                    }
                    stones[j] = CalculateBlink(stone, position);
                }

                //foreach (int stone in stones)
                //{
                //    Console.Write(stone + " ");
                //}
                //Console.WriteLine();
            }

            answer = stones.Count;

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();

            foreach (long stone in stones)
            {
                if (!stoneCounts.ContainsKey(stone))
                {
                    stoneCounts.Add(stone, 1);
                }
                else
                {
                    stoneCounts[stone]++;
                }
            }

            for (int i = 0; i < blinkCount; i++)
            {
                Dictionary <long, long> newStoneCounts = new Dictionary<long, long>();

                long keyCount = stoneCounts.Keys.Count;
                for(int j = 0; j < keyCount;j++)
                {
                    long stone = stoneCounts.Keys.ElementAt(j);
                    long count = stoneCounts[stone];
                    long[] newStones;

                    if (memoization.ContainsKey(stone))
                    {
                        newStones = memoization[stone];
                    }
                    else
                    {
                        newStones = StepStone(stone);
                        memoization.Add(stone, newStones);
                    }
                         
                    if (!newStoneCounts.ContainsKey(newStones[0]))
                    {
                        newStoneCounts.Add(newStones[0], count);
                    }
                    else
                    {
                        newStoneCounts[newStones[0]]+= count;
                    }
                    if (newStones[1] != -1)
                    {
                        if (!newStoneCounts.ContainsKey(newStones[1]))
                        {
                            newStoneCounts.Add(newStones[1], count);
                        }
                        else
                        {
                            newStoneCounts[newStones[1]]+= count;
                        }
                    }
                    
                }

                stoneCounts = newStoneCounts;
            }

            stoneCount = stoneCounts.Values.Sum();




            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, stoneCount);
        }

        

        private static long[] StepStone(long stoneValue)
        {
            if (stoneValue == 0)
            {
                return new long[] { 1, -1 };
            }
            else if (IsEvenDigits(stoneValue))
            {
                return new long[] { GetLeftDigits(stoneValue), GetRightDigits(stoneValue) };
            }
            else
            {
                return new long[] { stoneValue * 2024, -1 };
            }
        }
        private static long CalculateBlink(long stoneValue, int position)
        {
            if (stoneValue == 0)
            {
                return 1;
            }
            else if (IsEvenDigits(stoneValue))
            {
                stones.Insert(position + 1, GetRightDigits(stoneValue));
                skipPosition = position + 1;
                return GetLeftDigits(stoneValue);
            }
            else
            {
                return stoneValue * 2024;
            }
        }

        private static bool IsEvenDigits(long number)
        {
            int digitCnt = 0;
            while (number > 0)
            {
                number /= 10; // Remove the last digit
                digitCnt++;
            }

            if (digitCnt % 2 != 0)
            {
                return false; // Odd number of digits
            }
            return true; // even number of digits
        }

        private static long GetLeftDigits(long number)
        {
            string numberStr = number.ToString();
            int totalDigits = numberStr.Length;
            int leftHalfLength = totalDigits / 2;
            string leftHalfString = numberStr.Substring(0, leftHalfLength);
            if (long.TryParse(leftHalfString, out long leftHalf))
            {
                return leftHalf;
            }
            else
            {
                return -1; // Return -1 if parsing fails
            }
        }

        private static long GetRightDigits(long number)
        {
            string numberStr = number.ToString();
            int totalDigits = numberStr.Length;
            int rightHalfLength;
            if (totalDigits % 2 == 0)
            {
                rightHalfLength = totalDigits / 2;
            }
            else
            {
                rightHalfLength = (totalDigits + 1) / 2; // Round up for odd lengths
            }
            string rightHalfStr = numberStr.Substring(totalDigits - rightHalfLength, rightHalfLength);
            if (long.TryParse(rightHalfStr, out long rightHalf))
            {
                return rightHalf;
            }
            else
            {
                return -1;
            }
        }
    }
}