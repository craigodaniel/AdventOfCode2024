using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day01
    {
        public static string fileDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AocInputs";
        public static string fileName = "day01.txt";
        public static string[] lines = File.ReadAllLines(fileDir + "\\" + fileName);
        public static void Part1()
        {
            Console.WriteLine("Part1");

            int answer = 0;
            int[] firstArray = GetNumberList(lines,0);
            int[] secondArray = GetNumberList(lines,3);

            Array.Sort(firstArray);
            Array.Sort(secondArray);

            for (int i = 0; i < firstArray.Length; i++)
            {
                answer += Math.Abs(secondArray[i] - firstArray[i]);
            }
            
            Console.WriteLine(answer);
            
        }

        public static void Part2() 
        {
            Console.WriteLine("Part2");
            int answer = 0;
            int[] firstArray = GetNumberList(lines, 0);
            int[] secondArray = GetNumberList(lines, 3);

            Array.Sort(firstArray);
            Array.Sort(secondArray);

            for (int i = 0; i < firstArray.Length; i++)
            {
                answer += firstArray[i] * CountMatches(firstArray[i], secondArray);
            }

            Console.WriteLine(answer);

        }


        private static int[] GetNumberList(string[] lines, int position)
        {
            int[] numberPairs = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(" ");


                numberPairs[i] = ParseStringToInt(line[position]);
            }

            return numberPairs;
        }


        private static int ParseStringToInt(string input)
        {
            try
            {
                int result = int.Parse(input);
                return result;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        private static int CountMatches(int input, int[] secondArray)
        {
            int cnt = 0;
            for (int i = 0; i < secondArray.Length; i++)
            {
                if (input == secondArray[i])
                {
                    cnt++;
                }
            }
            return cnt;
        }
    }
}
