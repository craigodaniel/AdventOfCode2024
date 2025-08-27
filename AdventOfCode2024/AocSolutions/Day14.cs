using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day14
    {
        // --- Day 14:  ---
        // https://adventofcode.com/2024/day/14
        //
        // Part 1 runtime: 3.2522ms. The answer is: 229980828
        // Part 2 runtime: 221746.0018ms. The answer is: 7132
        //
        // Comments: 

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day14_sample.txt";
        private static string fileName = "day14_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        private static List<Robot> Robots = new List<Robot>();
        private static int gridWidth = 101;
        private static int gridHeight = 103;
        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            GetRobots();
            for (int i = 0; i < 100; i++)
            {
                
                MoveRobots();
            }

            int robotsInQuadrantOne = CountRobotsInQuadrant(0, gridWidth / 2 - 1, 0, gridHeight / 2 - 1);
            int robotsInQuadrantTwo = CountRobotsInQuadrant(gridWidth / 2 + 1, gridWidth - 1, 0, gridHeight / 2 - 1);
            int robotsInQuadrantThree = CountRobotsInQuadrant(0, gridWidth / 2 - 1, gridHeight / 2 + 1, gridHeight - 1);
            int robotsInQuadrantFour = CountRobotsInQuadrant(gridWidth / 2 + 1, gridWidth - 1, gridHeight / 2 + 1, gridHeight - 1);

            //Console.WriteLine("Robots in Quadrant 1: {0}", robotsInQuadrantOne);
            //Console.WriteLine("Robots in Quadrant 2: {0}", robotsInQuadrantTwo);
            //Console.WriteLine("Robots in Quadrant 3: {0}", robotsInQuadrantThree);
            //Console.WriteLine("Robots in Quadrant 4: {0}", robotsInQuadrantFour);

            answer = robotsInQuadrantOne * robotsInQuadrantTwo * robotsInQuadrantThree * robotsInQuadrantFour;

            //PrintGrid();

            

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            GetRobots();
            for (int i = 0; i < 1000000000; i++)
            {
                MoveRobots();
                if (CountRobotsInLine())
                {
                    Console.WriteLine("Robots aligned after {0} moves.", i + 1);
                    PrintGrid();
                    answer = i + 1;
                    break;
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static void GetRobots()
        {
            foreach (string line in lines)
            {
                // p=0,4 v=3,-3
                string[] parts = line.Split(new string[] { "p=", ",", " v=", ","}, StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                int vx = int.Parse(parts[2]);
                int vy = int.Parse(parts[3]);
                Robots.Add(new Robot(x, y, vx, vy));
            }
        }

        private static void MoveRobots()
        {
            foreach (Robot robot in Robots)
            {
                robot.X += robot.Vx;
                robot.Y += robot.Vy;

                if (robot.X < 0) 
                {
                    robot.X = gridWidth + robot.X;
                }
                else if (robot.X >= gridWidth)
                {
                    robot.X = robot.X - gridWidth;
                }
                if (robot.Y < 0)
                {
                    robot.Y = gridHeight + robot.Y;
                }
                else if (robot.Y >= gridHeight)
                {
                    robot.Y = robot.Y - gridHeight;
                }
            }
        }

        private static void PrintGrid()
        {
            char[,] grid = new char[gridHeight, gridWidth];
            for (int r = 0; r < gridHeight; r++)
            {
                for (int c = 0; c < gridWidth; c++)
                {
                    grid[r, c] = '.';
                }
            }
            foreach (Robot robot in Robots)
            {
                grid[robot.Y, robot.X] = '#';
            }
            for (int r = 0; r < gridHeight; r++)
            {
                for (int c = 0; c < gridWidth; c++)
                {
                    Console.Write(grid[r, c]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static int CountRobotsInQuadrant(int xMin, int xMax, int yMin, int yMax)
        {
            int count = 0;
            foreach (Robot robot in Robots)
            {
                if (robot.X >= xMin && robot.X <= xMax && robot.Y >= yMin && robot.Y <= yMax)
                {
                    count++;
                }
            }
            return count;
        }

        private static bool CountRobotsInLine()
        {

            int count = 0;
            
            for (int i = 0; i < gridHeight; i++)
            {
                count = 0;
                for (int j = 0; j < gridWidth; j++)
                {
                    bool found = false;
                    foreach (Robot robot in Robots)
                    {
                        if (robot.X == j && robot.Y == i)
                        {
                            count++;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        count = 0;
                    }
                    if (count >= 10)
                    {
                        return true;
                    }
                }
                
            }
            
            return false;
        }
    }


    public class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Vx { get; set; }
        public int Vy { get; set; }

        public Robot(int x, int y, int vx, int vy)
        {
            X = x;
            Y = y;
            Vx = vx;
            Vy = vy;
        }
    }
}