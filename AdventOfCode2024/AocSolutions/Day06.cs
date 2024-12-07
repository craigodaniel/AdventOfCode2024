using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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
        // the Enumerable.SequenceEqual method and System.Linq. Need to play with this some more...
        //
        // Came back to try and optimize Part 2 runtime. Original solution runtime was 4055.0693ms
        // In GetNextPosition() changed from recursive function to while loop. Part 2 runtime: 3851.4088ms. The answer is: 1688
        // Instead of testing obstacle position on entire map, try only the guard's original path. Trys only 4665 tiles instead of 16000
        // Part 2 runtime: 649.2263ms. The answer is: 1688


        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day06_sample.txt";
        private static string fileName = "day06_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static char direction = '^';
        private static bool isLoop = false;
        private static List<int[]> corners = new List<int[]>();
        private static List<int[]> visitedTiles = new List<int[]>();

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            char[,] map = AocLib.GetChar2dArrayFromStringArray(lines);
            visitedTiles.Clear();

            PlayMap(map, true);
            answer = visitedTiles.Count();


            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            char[,] originalMap = AocLib.GetChar2dArrayFromStringArray(lines);
            visitedTiles.Clear();

            //Set visitedTiles from Part 1
            PlayMap(originalMap, true);

            foreach (int[] tile in visitedTiles)
            {
                if (originalMap[tile[0],tile[1]] == '.')
                {
                    originalMap[tile[0], tile[1]] = '#'; //add obstacle
                    PlayMap(originalMap, false);
                    if (isLoop)
                    {
                        answer++;
                    }
                    originalMap[tile[0], tile[1]] = '.'; //remove obstacle
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static int[] GetNextPosition(char[,] map, int[] position)
        {
            int[] nextPosition = { position[0], position[1] };
            bool isValid = false;

            while (!isValid)
            {
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
                    nextPosition[0] = position[0];
                    nextPosition[1] = position[1];
                }
                else
                {
                    isValid = true;
                }

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

        private static void PlayMap(char[,] map, bool trackVisitedTiles = false)
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
                        row = rowMax;
                        col = colMax;
                    }
                }
            }

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
                    if (trackVisitedTiles)
                    {
                        AddToVisitedTiles(position[0],position[1]);
                        AddToVisitedTiles(nextPosition[0],nextPosition[1]);
                    }
                    winCond = true;
                }                
                else
                {
                    if (trackVisitedTiles)
                    {
                        AddToVisitedTiles(position[0], position[1]);
                    }
                    position[0] = nextPosition[0];
                    position[1] = nextPosition[1];
                }
            }
        }

        private static void AddToVisitedTiles(int x, int y)
        {
            bool inList = false;

            foreach (int[] tile in visitedTiles)
            {
                if (tile[0] ==x && tile[1] == y)
                {
                    inList = true;
                    break;
                }
            }
            if (!inList) { visitedTiles.Add(new int[] { x,y}); }
        }
    }
}