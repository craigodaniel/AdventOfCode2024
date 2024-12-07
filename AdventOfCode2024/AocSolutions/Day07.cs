using System.Diagnostics;

namespace AdventOfCode2024
{
    public class Day07
    {
        // --- Day 7: Bridge Repair ---
        // https://adventofcode.com/2024/day/7
        //
        // Part 1 runtime: 23.5747ms. The answer is: 1298300076754
        // Part 2 runtime: 281.2729ms. The answer is: 248427118972289
        //
        // Comments: stack is love. stack is life.

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day07_sample.txt";
        private static string fileName = "day07_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;

            List<long[]> listOfInputs = GetListOfLongArraysFromInput(lines);

            foreach (long[] input in listOfInputs)
            {
                answer += EvaluateLine(input);
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();
            long answer = 0;

            List<long[]> listOfInputs = GetListOfLongArraysFromInput(lines);

            foreach (long[] input in listOfInputs)
            {
                answer += EvaluateLine2(input);
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static List<long[]> GetListOfLongArraysFromInput(string[] input)
        {
            List<long[]> listOfInputs = new List<long[]>();
            foreach (string line in input)
            {
                string[] s = line.Split(':');
                s[1] = s[1].Trim();
                string[] s2 = s[1].Split(' ');

                long[] numbers = new long[s2.Length + 1];
                numbers[0] = long.Parse(s[0]);
                for (long i = 0; i < s2.Length; i++)
                {
                    numbers[i+1] = long.Parse(s2[i]);
                }
                listOfInputs.Add(numbers);
            }
            return listOfInputs;
        }

        private static long EvaluateLine(long[] input)
        {
            Stack<long[]> stack = new Stack<long[]>();
            stack.Push(input);

            while (stack.Count > 0)
            {
                long[] equation = stack.Pop();
                long index = 0;

                for (long i = 2;i < equation.Length; i++)
                {
                    if (equation[i] != -1) // I'm using -1 to flag an index as already evaluated
                    {
                        index = i;
                        break;
                    }
                }

                if (index == 0)
                {
                    if (equation[0] == equation[1])
                    {
                        //equation is valid
                        return equation[0];
                    }
                }
                else
                {
                    long[] mulEquation = new long[equation.Length];
                    long[] addEquation = new long[equation.Length];
                    equation.CopyTo(mulEquation, 0);
                    equation.CopyTo(addEquation, 0);

                    mulEquation[1] = mulEquation[1] * equation[index];
                    addEquation[1] = addEquation[1] + equation[index];

                    if (mulEquation[1] <= equation[0])
                    {
                        mulEquation[index] = -1;
                        stack.Push(mulEquation);
                    }

                    if (addEquation[1] <= equation[0])
                    {
                        addEquation[index] = -1;
                        stack.Push(addEquation);
                    }
                }
            }

            return 0;
        }

        private static long EvaluateLine2(long[] input)
        {
            Stack<long[]> stack = new Stack<long[]>();
            stack.Push(input);

            while (stack.Count > 0)
            {
                long[] equation = stack.Pop();
                long index = 0;

                for (long i = 2; i < equation.Length; i++)
                {
                    if (equation[i] != -1)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == 0)
                {
                    if (equation[0] == equation[1])
                    {
                        //equation is valid
                        return equation[0];
                    }
                }
                else
                {
                    long[] mulEquation = new long[equation.Length];
                    long[] addEquation = new long[equation.Length];
                    long[] concatEquation = new long[equation.Length];

                    equation.CopyTo(mulEquation, 0);
                    equation.CopyTo(addEquation, 0);
                    equation.CopyTo(concatEquation, 0);

                    mulEquation[1] = mulEquation[1] * equation[index];
                    addEquation[1] = addEquation[1] + equation[index];
                    string s = concatEquation[1].ToString() + equation[index].ToString();
                    concatEquation[1] = long.Parse(s);

                    if (mulEquation[1] <= equation[0])
                    {
                        mulEquation[index] = -1;
                        stack.Push(mulEquation); 
                    }

                    if (addEquation[1] <= equation[0])
                    {
                        addEquation[index] = -1;
                        stack.Push(addEquation);
                    }

                    if (concatEquation[1] <= equation[0])
                    {
                        concatEquation[index] = -1;
                        stack.Push(concatEquation);
                    }
                }
            }

            return 0;
        }


    }
}