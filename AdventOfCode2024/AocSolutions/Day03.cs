using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Markup;

namespace AdventOfCode2024
{
    public partial class Day03
    {
        // --- Day 3: Mull It Over ---
        // https://adventofcode.com/2024/day/3
        //
        // Part 1 runtime: 9.9858ms. The answer is: 159892596
        // Part 2 runtime: 7.6332ms. The answer is: 92626942
        //
        // Comments: Been avoiding Regex for too long...time to dive in.
        // 
        // First try Part 1 runtime: 18.0285ms. The answer is: 29728992. Answer is too low... last match was Found 'mul(946,270)' at position 2941. The input file contains 17,894 characters. Am I hiting a string limit?
        // ahh the input is more than one line...
        // Second try Part 1 runtime: 25.2053ms. The answer is: 159892596...Success!
        //
        // First try Part 2 runtime: 5.2254ms. The answer is: 99812796. Answer is too high...hmm maybe indexDo and indexDont aren't reseting on next line?...Nope same answer.
        // ok...I think i got it. Need to save the state of do/don't when going to the next line in the input...
        // Found 'mul(361,116)' at position 2939
        // indexDo: 1807, indexDont: 2781, mul skipped!     <---- last state was don't
        // Found 'mul(552,797)' at position 3
        // indexDo: 0, indexDont: 0, answer + (552*797) = 51357523 <---- breaks here because do was assumed at start of line.  Rechecked puzzle prompt and do is only assumed at start of program.
        // honestly easiest way to fix this may be to append each line into one continuous string.
        // Second try Part 2 runtime: 7.6332ms. The answer is: 92626942...Success!

        private static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        //private static string fileName = "day03_sample.txt";
        private static string fileName = "day03_actual.txt";
        private static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);


        public static void Part1() 
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            string pattern = "mul\\([0-9]+,[0-9]+\\)";  // finds patterns of mul(#,#) ex. mul(11,8)
            Regex regex = new Regex(pattern);
            
            foreach (string line in lines)
            {
                foreach (Match match in regex.Matches(line))
                { 
                    //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);

                    string value = match.Value; // ex. 'mul(2,4)'
                    value = value.Substring(4, value.Length - 5); // removes 'mul(' and ')'...ex. '2,4'
                    int[] values = AocLib2024.AocLib.GetIntArrayFromString(value, ",");


                    answer += values[0] * values[1];


                }
            }
            

            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 1 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);

        }

        public static void Part2() 
        {
            long startTime = Stopwatch.GetTimestamp();
            int answer = 0;

            string pattern = "mul\\([0-9]+,[0-9]+\\)"; // finds patterns of mul(#,#) ex. mul(11,8)
            Regex regex = new Regex(pattern);

            string line = "";

            foreach (string row in lines) // fixes broken input...it is intended to be one long string.
            {
                line = line + row;
            }



            foreach (Match match in regex.Matches(line))
            {
                //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);

                //Search right-to-left from match index and find first don't() and do() and compare their indexs
                string lookbehind = line.Substring(0, match.Index);
                int indexDo = new Regex("do\\(\\)", RegexOptions.RightToLeft).Match(lookbehind).Index; //returns 0 if not found
                int indexDont = new Regex("don't\\(\\)", RegexOptions.RightToLeft).Match(lookbehind).Index; //returns 0 if not found

                // If do() comes before mul() then do the mul!
                if (indexDo >= indexDont)
                {
                    string value = match.Value; // ex. 'mul(2,4)'
                    value = value.Substring(4, value.Length - 5); // removes 'mul(' and ')'...ex. '2,4'
                    int[] values = AocLib2024.AocLib.GetIntArrayFromString(value, ",");


                    answer += values[0] * values[1];
                    //Console.WriteLine("indexDo: {0}, indexDont: {1}, answer + ({2}*{3}) = {4}", indexDo, indexDont, values[0], values[1], answer);
                }
                else
                {
                    //Console.WriteLine("indexDo: {0}, indexDont: {1}, mul skipped!", indexDo, indexDont);
                }
                    
                    

            }


            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
            Console.WriteLine("Part 2 runtime: {0}ms. The answer is: {1}", elapsedTime.TotalMilliseconds, answer);
        }


        
    }
}
