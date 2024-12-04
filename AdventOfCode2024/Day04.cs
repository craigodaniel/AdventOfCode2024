using AdventOfCode2024.AocLib2024;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day04
    {
        // --- Day 4: Ceres Search ---
        // https://adventofcode.com/2024/day/4
        //
        // Part 1 runtime: 1.7944ms. The answer is: 2569
        // Part 2 runtime: 2.1323ms. The answer is: 1998
        //
        // Comments: 
        // First try Part 1 runtime: 2.8715ms. The answer is: 2538...answer too low.
        // I feel like it's probably literal edge cases. Going to make a test with only edge cases.
        // There are two kinds of errors, off by one errors...works now!
        // First try Part 2 runtime: 2.1323ms. The answer is: 1998...Correct!


        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day04_sample.txt";
        private static string fileName = "day04_actual.txt";
        //private static string fileName = "day04_edges.txt";
        //private static string fileName = "day04_edges2.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            char[,] wordSearch = AocLib.GetChar2dArrayFromStringArray(lines);
            int rowMin = 0;
            int rowMax = wordSearch.GetLength(0) - 1;
            int colMin = 0;
            int colMax = wordSearch.GetLength(1) - 1;
            int cntXmas = 0;

            for (int row = rowMin; row <= rowMax; row++)
            {
                for (int col = colMin; col <= colMax; col++)
                {
                    bool isXmas = false;
                    if (wordSearch[row, col] == 'X')
                    {
                        cntXmas += CountXmas(wordSearch, row, col, rowMin, rowMax, colMin, colMax);
                    }
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, cntXmas);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            char[,] wordSearch = AocLib.GetChar2dArrayFromStringArray(lines);
            int rowMin = 0;
            int rowMax = wordSearch.GetLength(0) - 1;
            int colMin = 0;
            int colMax = wordSearch.GetLength(1) - 1;
            int cntCrossMas = 0;

            for (int row = rowMin + 1; row < rowMax; row++)
            {
                for (int col = colMin + 1; col < colMax; col++)
                {
                    bool isCrossMas = false;
                    if (wordSearch[row, col] == 'A')
                    {
                        cntCrossMas += CountCrossMas(wordSearch, row, col, rowMin, rowMax, colMin, colMax);
                    }
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, cntCrossMas);
        }

        private static int CountXmas(char[,] wordSearch, int row, int col, int rowMin, int rowMax, int colMin, int colMax)
        {
            int matchCnt = 0;
            bool canBeRight = false;
            bool canBeLeft = false;
            bool canBeUp = false;
            bool canBeDown = false;

            if (col <= colMax - 3) { canBeRight = true; }
            if (col >= colMin + 3) { canBeLeft = true; }
            if (row <= rowMax - 3) { canBeDown = true; }
            if (row >= rowMin + 3) { canBeUp = true; }

            if (canBeRight)
            {
                bool isRight = CheckRight(wordSearch, row, col);
                if (isRight) 
                {
                    matchCnt++;
                    //Console.WriteLine("XMAS found at [{0},{1}] right!", row, col); 
                }
            }

            if (canBeLeft)
            {
                bool isLeft = CheckLeft(wordSearch, row, col);
                if (isLeft) 
                {
                    matchCnt++;
                    //Console.WriteLine("XMAS found at [{0},{1}] left!", row, col); 
                }
            }

            if (canBeDown)
            {
                bool isDown = CheckDown(wordSearch, row, col);
                if (isDown) 
                { 
                    matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] down!", row, col); 
                }
            }

            if (canBeUp)
            {
                bool isUp = CheckUp(wordSearch, row, col);
                if (isUp) 
                { matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] up!", row, col); 
                }
            }

            if (canBeUp && canBeLeft)
            {
                bool isUpLeft = CheckUpLeft(wordSearch, row, col);
                if (isUpLeft) 
                { 
                    matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] up left!",row,col); 
                }
            }

            if (canBeUp && canBeRight) 
            {
                bool isUpRight = CheckUpRight(wordSearch, row, col);
                if (isUpRight) 
                { 
                    matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] up right!", row, col); 
                }
            }

            if (canBeDown && canBeLeft)
            {
                bool isDownLeft = CheckDownLeft(wordSearch, row, col);
                if (isDownLeft) 
                {
                    matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] down left!", row, col); 
                }
            }

            if (canBeDown && canBeRight)
            {
                bool isDownRight = CheckDownRight(wordSearch, row, col);
                if (isDownRight) 
                { 
                    matchCnt++; 
                    //Console.WriteLine("XMAS found at [{0},{1}] down right!", row, col); 
                }
            }

            return matchCnt;
        }

        private static bool CheckRight(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row, col + 1] == 'M' && 
                wordSearch[row, col + 2] == 'A' &&
                wordSearch[row, col + 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckLeft(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row, col - 1] == 'M' &&
                wordSearch[row, col - 2] == 'A' &&
                wordSearch[row, col - 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckDown(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row + 1, col] == 'M' &&
                wordSearch[row + 2, col] == 'A' &&
                wordSearch[row + 3, col] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckUp(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row - 1, col] == 'M' &&
                wordSearch[row - 2, col] == 'A' &&
                wordSearch[row - 3, col] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckUpLeft(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row - 1, col - 1] == 'M' &&
                wordSearch[row - 2, col - 2] == 'A' &&
                wordSearch[row - 3, col - 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckUpRight(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row - 1, col + 1] == 'M' &&
                wordSearch[row - 2, col + 2] == 'A' &&
                wordSearch[row - 3, col + 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckDownLeft(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row + 1, col - 1] == 'M' &&
                wordSearch[row + 2, col - 2] == 'A' &&
                wordSearch[row + 3, col - 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static bool CheckDownRight(char[,] wordSearch, int row, int col)
        {
            bool isXmas = false;

            if (
                wordSearch[row + 1, col + 1] == 'M' &&
                wordSearch[row + 2, col + 2] == 'A' &&
                wordSearch[row + 3, col + 3] == 'S'
                )
            {
                isXmas = true;
            }

            return isXmas;
        }

        private static int CountCrossMas(char[,] wordSearch, int row, int col, int rowMin, int rowMax, int colMin, int colMax)
        {
            int matchCnt = 0;
            bool backSlash = false;
            bool fwdSlash = false;

            //Check BackSlash
            switch (wordSearch[row - 1, col - 1])
            {
                case 'M':
                    if (wordSearch[row + 1, col + 1] == 'S'){ backSlash = true; }
                    break;

                case 'S':
                    if (wordSearch[row + 1, col + 1] == 'M') { backSlash = true; }
                    break;
                default:
                    backSlash = false;
                    break;
            }

            //Check FwdSlash
            switch (wordSearch[row + 1, col - 1])
            {
                case 'M':
                    if (wordSearch[row - 1, col + 1] == 'S') { fwdSlash = true; }
                    break;

                case 'S':
                    if (wordSearch[row - 1, col + 1] == 'M') { fwdSlash = true; }
                    break;
                default:
                    fwdSlash = false;
                    break;
            }

            if (backSlash == true && fwdSlash == true)
            {
                matchCnt++;
                //Console.WriteLine("X-MAS found at [{0},{1}]!", row, col);
            }

            

            return matchCnt;
        }
    }
}
