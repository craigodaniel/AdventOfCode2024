using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.AocLib2024;

namespace AdventOfCode2024
{
    public class Day05
    {
        // --- Day 5: Print Queue ---
        // https://adventofcode.com/2024/day/5
        //
        // Part 1 runtime: 2.9648ms. The answer is: 4996
        // Part 2 runtime: 28.5946ms. The answer is: 6311
        //
        // Comments: 
        // Part 1 got it first try! Lots of debugging on the sample input...lots of comments
        // Part 2 got first try too! 
        // There are 21 'if' statements in this solution...I counted.

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day05_sample.txt";
        private static string fileName = "day05_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);

        private static Dictionary<int, List<int>> beforeRules = new Dictionary<int, List<int>>();
        private static Dictionary<int, List<int>> afterRules = new Dictionary<int, List<int>>();
        private static List<List<int>> badUpdates = new List<List<int>>();
        public static void Part1()
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;
            beforeRules.Clear();
            afterRules.Clear();
            badUpdates.Clear();            
            List<int> pageUpdates = new List<int>();

            foreach (string line in lines)
            {
                 
                if (line.Length == 5) // is rule (ex. 47|53)
                {
                    int[] xy = AocLib.GetIntArrayFromString(line, "|");

                    // Dict for X|Y page order rules
                    if (!beforeRules.ContainsKey(xy[0]))
                    {
                        beforeRules.Add(xy[0], new List<int>());
                        
                    }
                    beforeRules[xy[0]].Add(xy[1]);

                    //Dict for Y|X page order rules
                    if (!afterRules.ContainsKey(xy[1]))
                    {
                        afterRules.Add(xy[1], new List<int>());
                    }
                    afterRules[xy[1]].Add(xy[0]);

                }
                else if (line.Length > 5)
                {
                    pageUpdates = AocLib.GetListOfIntsFromString(line, ",");
                    bool isUpdateGood = CheckIfGoodUpdate(pageUpdates);
                    

                    if (isUpdateGood)
                    {
                        //Console.WriteLine("Line is good:");
                        //foreach (int page in pageUpdates) { Console.Write(page + ","); }
                        answer += GetMiddleNumber(pageUpdates);
                    }
                    else
                    {
                        //Console.WriteLine("Line is bad:");
                        //foreach (int page in pageUpdates) { Console.Write(page + ","); }
                        //Console.WriteLine();
                        badUpdates.Add(pageUpdates);
                    }
                }
            }

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        public static void Part2()
        {
            long startTime = Stopwatch.GetTimestamp();

            Part1(); // run part 1 to get list of bad updates and rules dicts
            int answer = 0;

            foreach(List<int> pageUpdates in badUpdates)
            {
                answer += GetMiddleNumber(SortPagesByRules(pageUpdates));
            }


            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }

        private static List<int> SortPagesByRules(List<int> pageUpdates)
        {
            foreach (int x in pageUpdates)
            {
                bool swapped = false;
                // check rules before
                if (beforeRules.ContainsKey(x))
                {
                    foreach (int y in beforeRules[x])
                    {
                        if (pageUpdates.Contains(y))
                        {
                            if (pageUpdates.IndexOf(x) > pageUpdates.IndexOf(y)) //true if breaks rule
                            {
                                //swap x and y and exit foreach loop
                                int yIndex = pageUpdates.IndexOf(y);
                                int xIndex = pageUpdates.IndexOf(x);

                                pageUpdates[xIndex] = y;
                                pageUpdates[yIndex] = x;
                                swapped = true;
                                break;
                            }
                        }

                    }
                }
                if (swapped) { break; }

                // check rules after
                if (afterRules.ContainsKey(x))
                {
                    foreach (int y in afterRules[x])
                    {
                        if (pageUpdates.Contains(y))
                        {
                            if (pageUpdates.IndexOf(x) < pageUpdates.IndexOf(y)) //true if breaks rule
                            {
                                //swap x and y and exit foreach loop
                                int yIndex = pageUpdates.IndexOf(y);
                                int xIndex = pageUpdates.IndexOf(x);

                                pageUpdates[xIndex] = y;
                                pageUpdates[yIndex] = x;
                                swapped = true;
                                break;
                            }
                        }

                    }
                }
                if (swapped) { break; }
            }


            if (CheckIfGoodUpdate(pageUpdates))
            {
                return pageUpdates;
            }
            else
            {
                pageUpdates = SortPagesByRules(pageUpdates);
                return pageUpdates;
            }
        }

        private static bool CheckIfGoodUpdate(List<int> pageUpdates)
        {
            bool isUpdateGood = false;
            // for each page number
            foreach (int x in pageUpdates)
            {
                bool isGoodBefore = false;
                bool isGoodAfter = false;

                // check rules before
                if (beforeRules.ContainsKey(x))
                {
                    foreach (int y in beforeRules[x])
                    {
                        if (pageUpdates.Contains(y))
                        {
                            if (pageUpdates.IndexOf(x) < pageUpdates.IndexOf(y))
                            {
                                isGoodBefore = true;
                            }
                            else
                            {
                                //Console.WriteLine("beforeRules fails {0}|{1}", x, y);
                                isGoodBefore = false;
                                break;
                            }
                        }
                        else
                        {
                            isGoodBefore = true;
                        }

                    }
                }
                else
                {
                    //Console.WriteLine("beforeRules doesn't contain key {0}", x);
                    isGoodBefore = true; //assumption made!
                }

                // check rules after
                if (afterRules.ContainsKey(x))
                {
                    foreach (int y in afterRules[x])
                    {
                        if (pageUpdates.Contains(y))
                        {
                            if (pageUpdates.IndexOf(x) > pageUpdates.IndexOf(y))
                            {
                                isGoodAfter = true;
                            }
                            else
                            {
                                //Console.WriteLine("afterRules fails {1}|{0}", x, y);
                                isGoodAfter = false;
                                break;
                            }
                        }
                        else
                        {
                            isGoodAfter = true;
                        }

                    }
                }
                else
                {
                    //Console.WriteLine("afterRules doesn't contain key {0}", x);
                    isGoodAfter = true; //assumption made!
                }

                // if good before and good after find middle and add to answer
                if (isGoodBefore && isGoodAfter)
                {
                    isUpdateGood = true;
                    //Console.WriteLine();
                    //foreach (int page in pageUpdates) {Console.Write(page + ","); }

                }
                else
                {
                    //Console.WriteLine("Bad update at {0}", x);
                    isUpdateGood = false;
                    break;
                }
            }

            return isUpdateGood;
        }

        private static int GetMiddleNumber(List<int> pageUpdates)
        {
            int midpoint = (pageUpdates.Count / 2); // odd number + 0 index makes this work
            //Console.WriteLine(" <-- middle is {0}", pageUpdates[midpoint]);
            return pageUpdates[midpoint];
        }
    }
}
