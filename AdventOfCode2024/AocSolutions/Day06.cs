using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day06
    {
        // --- Day 6: Guard Gallivant ---
        // https://adventofcode.com/2024/day/6
        //
        // Part 1 runtime: 2.4084ms. The answer is: 4665
        // Part 2 runtime: 4055.0693ms. The answer is: 1688
        //
        // Comments: 
        // Part 1 first try success!
        // Part 2 first try success! Approach was to check every possible variation of the map for loops.
        // Loops were found by keeping a list of "corners", i.e. when the guard hits a wall and has to turn.
        // If a corner has already been visited before (i.e. wall detected at the same position as before)
        // then you know it's a loop because making the turn will put you back on the same path.
        //
        // Ran into troubles during testing with trying to check if corners.Contains(corner). Didn't work
        // because the corners list did not contain the new int[] object. Had to do some googling to figure out
        // the Enemerable.SequenceEqual method and System.Linq. Need to play with this some more...



        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day06_sample.txt";
        private static string fileName = "day06_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static char direction = '^';
        private static bool isLoop = false;
        private static List<int[]> corners = new List<int[]>();

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            char[,] map = AocLib.GetChar2dArrayFromStringArray(lines);
            int[] position = { 0, 0 };
            direction = '^';
            int rowMax = map.GetLength(0) - 1;
            int colMax = map.GetLength(1) - 1;
            bool winCond = false;


            //Find guard start position
            for (int row = 0; row <= rowMax; row++) 
            {
                for (int col = 0; col <= colMax; col++)
                {
                    if (map[row, col] == direction)
                    {
                        position[0] = row;
                        position[1] = col;
                        //Console.WriteLine("Guard starts at postion ({0},{1})", position[0], position[1]);
                        row = rowMax;
                        col = colMax;
                    }
                }
            }

            int cnt = 0;
            while (!winCond)
            {
                
                int[] nextPosition = GetNextPosition(map, position);
                if (nextPosition[0] == rowMax || nextPosition[0] == 0 || nextPosition[1] == 0 || nextPosition[1] == colMax)
                {
                    //win cond
                    map[position[0], position[1]] = 'X';
                    map[nextPosition[0], nextPosition[1]] = 'X';
                    answer = CountVisited(map);
                    //PrintMap(map);
                    winCond = true;
                }
                else
                {
                    cnt++;
                    map[position[0], position[1]] = 'X';
                    position[0] = nextPosition[0];
                    position[1] = nextPosition[1];
                    map[position[0], position[1]] = direction;
                    //Console.WriteLine("Move {0}:", cnt);
                    //PrintMap(map);
                }
            }


            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            char[,] originalMap = AocLib.GetChar2dArrayFromStringArray(lines);

            for (int row = 0; row < originalMap.GetLength(0); row++)
            {
                for (int col = 0; col < originalMap.GetLength(1); col++)
                {
                    if (originalMap[row,col] == '.')
                    {
                        char[,] map = AocLib.GetChar2dArrayFromStringArray(lines);
                        map[row, col] = '#'; //add obstacle
                        PlayMap(map);
                        if (isLoop)
                        {
                            answer++;
                        }
                    }
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static int[] GetNextPosition(char[,] map, int[] position)
        {
            int[] nextPosition = { position[0], position[1] };

            switch (direction)
            {
                case '^':
                    nextPosition[0]--;
                    break;
                case 'v':
                    nextPosition[0]++;
                    break;
                case '>':
                    nextPosition[1]++;
                    break;
                case '<':
                    nextPosition[1]--;
                    break;
                default:
                    break;
            }

            if (map[nextPosition[0], nextPosition[1]] == '#')
            {
                int[] corner = { position[0], position[1], nextPosition[0], nextPosition[1] };
                //if (corners.Contains(corner))
                if (corners.FindIndex(l => Enumerable.SequenceEqual(corner, l)) >= 0)
                {
                    //Loop found!
                    isLoop = true;
                }
                else
                {
                    corners.Add(corner);
                }
                direction = TurnCW();
                nextPosition = GetNextPosition(map, position);
            }
            
            return nextPosition;
        }

        private static char TurnCW()
        {
            switch (direction)
            {
                case '^':
                    direction = '>';
                    break;
                case 'v':
                    direction = '<';
                    break;
                case '>':
                    direction = 'v';
                    break;
                case '<':
                    direction = '^';
                    break;
                default:
                    break;
            }

            return direction;
        }

        private static int CountVisited(char[,] map)
        {
            int count = 0;
            foreach (char x in map)
            {
                if (x == 'X')
                {
                    count++;
                }
            }
            return count;
        }

        private static void PrintMap(char[,] map)
        {
            
            for (int row = 0;  row < map.GetLength(0); row++)
            {
                StringBuilder sb = new StringBuilder();

                for (int col = 0; col < map.GetLength(1); col++)
                {
                    sb.Append(map[row, col]);
                }

                Console.WriteLine(sb.ToString());
            }
        }

        private static void PlayMap(char[,] map)
        {
            isLoop = false;
            corners.Clear();
            direction = '^';
            int[] position = { 0, 0 };            
            int rowMax = map.GetLength(0) - 1;
            int colMax = map.GetLength(1) - 1;
            bool winCond = false;


            //Find guard start position
            for (int row = 0; row <= rowMax; row++)
            {
                for (int col = 0; col <= colMax; col++)
                {
                    if (map[row, col] == direction)
                    {
                        position[0] = row;
                        position[1] = col;
                        //Console.WriteLine("Guard starts at postion ({0},{1})", position[0], position[1]);
                        row = rowMax;
                        col = colMax;
                    }
                }
            }

            int cnt = 0;
            while (!winCond)
            {

                int[] nextPosition = GetNextPosition(map, position);
                if (isLoop)
                {
                    winCond = true;
                    return;
                }
                else if (nextPosition[0] == rowMax || nextPosition[0] == 0 || nextPosition[1] == 0 || nextPosition[1] == colMax)
                {
                    //win cond
                    map[position[0], position[1]] = 'X';
                    map[nextPosition[0], nextPosition[1]] = 'X';
                    //PrintMap(map);
                    winCond = true;
                }                
                else
                {
                    cnt++;
                    map[position[0], position[1]] = 'X';
                    position[0] = nextPosition[0];
                    position[1] = nextPosition[1];
                    map[position[0], position[1]] = direction;
                    //Console.WriteLine("Move {0}:", cnt);
                    //PrintMap(map);
                }
            }
        }
    }
}